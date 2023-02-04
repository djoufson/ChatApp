using ChatApp.Mobile.Extensions;
namespace ChatApp.Mobile.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly ShellNavigationService _shell;
    [ObservableProperty] User _user;
	[ObservableProperty] ObservableCollection<Conversation> _conversations;
    [ObservableProperty] bool _isBusy;
    [ObservableProperty] private bool _isRefreshing;

    public HomeViewModel(
        User user,
        ShellNavigationService shell)
	{
        _shell = shell;
        _user = user;
		_conversations = new ();
	}

	public async Task LoadConversationsAsync()
    {
        IsBusy = true;
        try
        {
            var response = await MyClient.SendRequestAsync<BaseResponseDto<IEnumerable<ConversationWithoutEntities>>>(
                MyHttpMethods.GET, 
                ConversationsRoute, 
                content: null, 
                AuthToken);

            if (response is null)
            {
                IsBusy = false;
                await _shell.Current.DisplayAlert("Error", "Wrong Credentials", "OK");
                return;
            }

            IsBusy = false;

            foreach (var conversation in response.Data)
                Conversations.Add(conversation.AsConversation());
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            await _shell.Current.DisplayAlert("Error", e.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await Task.Delay(1500);
        IsRefreshing = false;
    }
}
