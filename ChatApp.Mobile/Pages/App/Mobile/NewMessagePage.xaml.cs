namespace ChatApp.Mobile.Pages.App.Mobile;

public partial class NewMessagePage : ContentPage
{
	private readonly NewMessageViewModel _viewModel;
    public NewMessagePage(NewMessageViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
		InitializeComponent();
	}

	private async void NewMessageTapped(object sender, ItemTappedEventArgs e)
	{
		await _viewModel.NavigateInboxCommand.ExecuteAsync(e.Item);
	}
}