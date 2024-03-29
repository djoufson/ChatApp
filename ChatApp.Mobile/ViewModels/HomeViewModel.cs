﻿using ChatApp.Mobile.Extensions;
namespace ChatApp.Mobile.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IDisplayService _displayService;
    private readonly ShellNavigationService _shell;
    [ObservableProperty] User _user;
	[ObservableProperty] ObservableCollection<Conversation> _conversations;
    [ObservableProperty] private bool _isRefreshing;
    
    public HomeViewModel(
        IGroupConnection groupConnection,
        IMessageConnection messageConnection,
        IOnlineStatusConnection onlineStatusConnection,
        IMessageStatusConnection messageStatusConnection,
        IDisplayService displayService,
        User user,
        ShellNavigationService shell) : base(
            groupConnection, 
            messageConnection, 
            onlineStatusConnection, 
            messageStatusConnection)
	{
        _displayService = displayService;
        _shell = shell;
        _user = user;
		_conversations = new ();

        Task.Run(async () => await LoadConversationsAsync());
	}

    private void SortConversations()
    {
        Conversations = new ObservableCollection<Conversation>(Conversations
            .OrderBy(c => c.LastMessage.SentAt)
            .Reverse());
    }
    private async Task LoadConversationsAsync()
    {
        try
        {
            var response = await MyClient.SendRequestAsync<BaseResponseDto<IEnumerable<ConversationWithoutEntities>>>(
                MyHttpMethods.GET,
                ConversationsRoute,
                content: null,
                AuthToken);

            if (response is null)
            {
                await _displayService.DisplayAlert("Error", "Wrong Credentials", "OK");
                return;
            }

            Conversations.Clear();
            foreach (var conversation in response.Data)
                Conversations.Add(conversation.AsConversation());
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            await _displayService.DisplayAlert("Error", e.Message, "OK");
        }
    }
    public async void UpdateView()
    {
        var tasks = new List<Task>();
        foreach (var conversation in Conversations)
            tasks.Add(Task.Run(() => conversation.OnPropertyChanged()));

        await Task.WhenAll(tasks);
    }

    #region OVERRIDES
    // When recieving a new Message
    protected override void OnMessageRecieved(object sender, RecievedMessageEventArg e)
    {
        base.OnMessageRecieved(sender, e);
        var conversation = Conversations.FirstOrDefault(c => c.ToUserEmail == e.IssuerEmail); 
        if(conversation is null)
        {
            conversation = new Conversation()
            {
                Messages = new Collection<MessageWithoutEntities>(),
            };
            Conversations.Add(conversation);
        }
        conversation.Messages.Add(e.Message);
        conversation.OnPropertyChanged();
        var index = Conversations.IndexOf(conversation);
        Conversations.Move(index, 0);
    }

    protected override void OnMessageDelivered(object sender, MessageDeliveredEventArgs e)
    {
        base.OnMessageDelivered(sender, e);
    }
    #endregion

    #region COMMANDS

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadConversationsAsync();
        //SortConversations();
        IsRefreshing = false;
    }


    [RelayCommand]
    private async Task SelectAsync(Conversation conversation)
    {
        if (conversation.LastMessage.ToUserName == App.UserName)
        {
            try
            {
                await MyClient.SendRequestAsync<BaseResponseDto>(MyHttpMethods.PUT, $"conversations/{conversation.Id}", null, AuthToken);
            }
            catch(Exception) { }
        }
        await _shell.GoToAsync($"{nameof(InboxPage)}?ConversationId={conversation.Id}&WithUserEmail={conversation.ToUserEmail}&WithUserName={conversation.ToUserName}"
            , new Dictionary<string, object> { { "Conversation", conversation } });
    }

    [RelayCommand]
    private Task NewMessageAsync()
    {
        return _shell.GoToAsync(nameof(NewMessagePage));
    }
    #endregion
}
