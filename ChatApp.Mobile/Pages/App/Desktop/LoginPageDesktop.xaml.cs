namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class LoginPageDesktop : ContentPage
{
	public LoginPageDesktop(LoginViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}