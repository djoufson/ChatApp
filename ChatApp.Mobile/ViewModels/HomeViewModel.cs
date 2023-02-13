using ChatApp.Mobile.Extensions;
namespace ChatApp.Mobile.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly ShellNavigationService _shell;
    [ObservableProperty] User _user;
	[ObservableProperty] ObservableCollection<Conversation> _conversations;
    [ObservableProperty] private bool _isRefreshing;
    
    public HomeViewModel(
        IGroupConnection groupConnection,
        IMessageConnection messageConnection,
        IOnlineStatusConnection onlineStatusConnection,
        IMessageStatusConnection messageStatusConnection,
        User user,
        ShellNavigationService shell) : base(
            groupConnection, 
            messageConnection, 
            onlineStatusConnection, 
            messageStatusConnection)
	{
        _shell = shell;
        _user = user;
		_conversations = new ();
	}

    private void SortConversations()
    {
        Conversations = new ObservableCollection<Conversation>(Conversations
            .OrderBy(c => c.LastMessage.SentAt)
            .Reverse());
    }

	public async Task LoadConversationsAsync()
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
                await _shell.Current.DisplayAlert("Error", "Wrong Credentials", "OK");
                return;
            }

            Conversations.Clear();
            foreach (var conversation in response.Data)
                Conversations.Add(conversation.AsConversation());
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            await _shell.Current.DisplayAlert("Error", e.Message, "OK");
        }
    }


    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadConversationsAsync();
        SortConversations();
        IsRefreshing = false;
    }


    [RelayCommand]
    private Task SelectAsync(Conversation conversation)
    {
        return _shell.GoToAsync($"{nameof(InboxPage)}?ConversationId={conversation.Id}&WithUserEmail={conversation.ToUserEmail}&WithUserName={conversation.ToUserName}");
    }


    [RelayCommand]
    private Task NewMessageAsync()
    {
        return _shell.GoToAsync(nameof(NewMessagePage));
    }
}
