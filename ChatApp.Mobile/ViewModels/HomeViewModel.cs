namespace ChatApp.Mobile.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    [ObservableProperty] User _user;

	public HomeViewModel(User user)
	{
		_user = user;
	}
}
