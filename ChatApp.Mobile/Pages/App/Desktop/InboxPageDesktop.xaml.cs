namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class InboxPageDesktop : ContentPage
{
    private readonly InboxViewModel _viewModel;
    public InboxPageDesktop(InboxViewModel viewModel)
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMessagesCommand.ExecuteAsync(this);
    }
}