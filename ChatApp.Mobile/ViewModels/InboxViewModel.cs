using ChatApp.Shared.Utilities.EventArgs;

namespace ChatApp.Mobile.ViewModels;


[QueryProperty(nameof(ConversationId), nameof(ConversationId))]
[QueryProperty(nameof(WithUserName), nameof(WithUserName))]
[QueryProperty(nameof(WithUserEmail), nameof(WithUserEmail))]
public partial class InboxViewModel : BaseViewModel
{
    [ObservableProperty] private int _conversationId;
    [ObservableProperty] private string _withUserName;
    [ObservableProperty] private string _withUserEmail;
    [ObservableProperty] private string _message;
    [ObservableProperty] private ObservableCollection<MessageWithoutEntities> _messages;
    
    private readonly IDisplayService _displayService;
    private readonly IGroupConnection _groupConnection;
    private readonly IMessageConnection _messageConnection;
    private readonly IOnlineStatusConnection _onlineStatusConnection;
    public event EventHandler OnLoadCompleted;

    // CONSTRUCTOR
    public InboxViewModel(
        IDisplayService displayService,
        IGroupConnection groupConnection,
        IMessageConnection messageConnection,
        IOnlineStatusConnection onlineStatusConnection,
        IMessageStatusConnection messageStatusConnection) : base(
            groupConnection,
            messageConnection,
            onlineStatusConnection,
            messageStatusConnection)
    {
        _displayService = displayService;
        _groupConnection = groupConnection;
        _messageConnection = messageConnection;
        _onlineStatusConnection = onlineStatusConnection;
        _messages = new ();
    }

    [RelayCommand]
    private async void Send()
    {
        var messageContent = new Dictionary<string, string>()
        {
            { "ToUserMail", WithUserEmail},
            { "Content", Message }
        };
        try
        {
            var response = await MyClient.SendRequestAsync<BaseResponseDto<MessageWithoutEntities>>(MyHttpMethods.POST, "message", messageContent, AuthToken);
            if (response is null) return;
            await _messageConnection.SendMessageToAsync(WithUserEmail, response.Data);
            Message = string.Empty;
            Messages.Add(response.Data);
        }
        catch(Exception e)
        {
            Debug.WriteLine(e);
            await _displayService.DisplayAlert("Error", e.Message, "OK");
        }
    }

    [RelayCommand]
    private async void LoadMessages()
    {
        IsBusy = true;
        BaseResponseDto<MessageWithoutEntities[]> response = null;
        try
        {
            if (string.IsNullOrEmpty(WithUserEmail))
                response = await MyClient.SendRequestAsync<BaseResponseDto<MessageWithoutEntities[]>>(
                    MyHttpMethods.GET, 
                    $"conversations/{ConversationId}/messages", 
                    null, 
                    AuthToken);
            else
                response = await MyClient.SendRequestAsync<BaseResponseDto<MessageWithoutEntities[]>>(
                    MyHttpMethods.GET, 
                    $"conversations/messages?userEmail={WithUserEmail}", 
                    null, 
                    AuthToken);

            if (response is null) return;

            if (response.Status)
                    foreach (var message in response.Data)
                        Messages.Add(message);
        }
        catch(Exception e)
        {
            Debug.WriteLine(e);
            await _displayService.DisplayAlert("Error", e.Message, "OK");
        }
        Focus();
        IsBusy = false;
    }

    [RelayCommand]
    private void Focus()
    {
        OnLoadCompleted?.Invoke(this, new EventArgs());
    }

    // When recieving a new Message
    protected override void OnMessageRecieved(object sender, RecievedMessageEventArg e)
    {
        base.OnMessageRecieved(sender, e);
        if (
            e.Message is null ||
            !e.Status ||
            e.IssuerEmail != WithUserEmail) 
            return;

        Messages.Add(e.Message);
    }

    // When the connectivity status changes
    protected override void OnlineStatusChanged(object sender, OnlineStatusChangedEventArgs e)
    {
        base.OnlineStatusChanged(sender, e);
        if (
            !e.Status ||
            e.ConversationId != ConversationId ||
            e.UserEmail != WithUserEmail) 
            return;

        // Update Status on the view
    }
}
