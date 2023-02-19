namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class InboxPageDesktop : ContentPage
{
    private readonly InboxViewModel _viewModel;
    public InboxPageDesktop(InboxViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadMessagesCommand.Execute(this);
    }
}