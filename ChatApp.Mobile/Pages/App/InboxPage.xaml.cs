namespace ChatApp.Mobile.Pages.App;

public partial class InboxPage : ContentPage
{
	private InboxViewModel _viewModel;
	public InboxPage(InboxViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadMessagesCommand.ExecuteAsync(this);
	}
}
