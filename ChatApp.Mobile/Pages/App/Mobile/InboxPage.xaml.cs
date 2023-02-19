namespace ChatApp.Mobile.Pages.App.Mobile;

public partial class InboxPage : ContentPage
{
	private readonly InboxViewModel _viewModel;
	public InboxPage(InboxViewModel viewModel)
	{
        _viewModel = viewModel;
		_viewModel.OnLoadCompleted += LoadCompleted;
        BindingContext = _viewModel;
		InitializeComponent();
	}

	private void LoadCompleted(object sender, EventArgs e)
	{
		messagesList.ScrollTo(_viewModel.Messages.Count - 1, -1, ScrollToPosition.MakeVisible, false);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadMessagesCommand.Execute(this);
	}
}
