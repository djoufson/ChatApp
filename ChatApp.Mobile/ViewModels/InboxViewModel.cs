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
        _messages = new ();
    }

    [RelayCommand]
    private async Task SendAsync()
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
    private async Task LoadMessagesAsync()
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
}
