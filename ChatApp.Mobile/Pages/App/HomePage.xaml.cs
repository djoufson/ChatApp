namespace ChatApp.Mobile.Pages.App;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}