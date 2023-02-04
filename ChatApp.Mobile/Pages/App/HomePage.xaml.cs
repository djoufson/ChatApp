namespace ChatApp.Mobile.Pages.App;

public partial class HomePage : ContentPage
{
	private bool _isFirstTime = true;
	public HomePage(HomeViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_isFirstTime)
		{
			await ((HomeViewModel)BindingContext).LoadConversationsAsync();
			_isFirstTime = false;
		}
	}
}