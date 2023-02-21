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

	private void LoadCompleted(object sender, bool animate)
	{
		messagesList.ScrollTo(_viewModel.Messages.Count - 1, -1, ScrollToPosition.MakeVisible, animate);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadMessagesCommand.Execute(this);
	}

	private async void HandleEditorFocused(object sender, FocusEventArgs e)
    {
		await Task.Delay(900);
		LoadCompleted(sender, true);
    }
}
