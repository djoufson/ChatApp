namespace ChatApp.Mobile.ViewModels;

[QueryProperty(nameof(ConversationId), nameof(ConversationId))]
public partial class InboxViewModel : BaseViewModel
{
    [ObservableProperty] private int _conversationId;
    [ObservableProperty] private ObservableCollection<MessageWithoutEntities> _messages;

    public InboxViewModel()
    {
        _messages = new ();
    }
    [RelayCommand]
    private async Task LoadMessages()
    {
        var response = await MyClient.SendRequestAsync<BaseResponseDto<MessageWithoutEntities[]>>(MyHttpMethods.GET, $"conversations/{ConversationId}/messages", null, AuthToken);
        if(response.Status)
            foreach (var message in response.Data)
                Messages.Add(message);
    }
}
