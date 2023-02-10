namespace ChatApp.Mobile.ViewModels;

[QueryProperty(nameof(ConversationId), nameof(ConversationId))]
[QueryProperty(nameof(WithUserName), nameof(WithUserName))]
public partial class InboxViewModel : BaseViewModel
{
    [ObservableProperty] private int _conversationId;
    [ObservableProperty] private string _withUserName;
    [ObservableProperty] private ObservableCollection<MessageWithoutEntities> _messages;

    public InboxViewModel()
    {
        _messages = new ();
    }
    [RelayCommand]
    private async Task LoadMessagesAsync()
    {
        var response = await MyClient.SendRequestAsync<BaseResponseDto<MessageWithoutEntities[]>>(MyHttpMethods.GET, $"conversations/{ConversationId}/messages", null, AuthToken);
        if(response.Status)
            foreach (var message in response.Data)
                Messages.Add(message);
    }
}
