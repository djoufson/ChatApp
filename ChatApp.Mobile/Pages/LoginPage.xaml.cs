using ChatApp.Mobile.ViewModels;

namespace ChatApp.Mobile.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}