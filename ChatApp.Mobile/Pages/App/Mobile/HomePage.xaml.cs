namespace ChatApp.Mobile.Pages.App.Mobile;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel _viewModel;
	public HomePage(HomeViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.UpdateView();
    }

	private async void ConversationTapped(object sender, ItemTappedEventArgs e)
	{
		var conversation = e.Item as Conversation;
		await _viewModel.SelectCommand.ExecuteAsync(conversation);
	}
}