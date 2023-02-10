namespace ChatApp.Mobile.Pages.App;

public partial class NewMessagePage : ContentPage
{
	public NewMessagePage(NewMessageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}