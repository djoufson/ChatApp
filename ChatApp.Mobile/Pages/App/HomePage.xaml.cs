namespace ChatApp.Mobile.Pages.App;

public partial class HomePage : ContentPage
{
	private HomeViewModel _viewModel;
	public HomePage(HomeViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
		InitializeComponent();
	}

	private async void ConversationTapped(object sender, ItemTappedEventArgs e)
	{
		var conversation = e.Item as Conversation;
		await _viewModel.SelectCommand.ExecuteAsync(conversation);
	}
}