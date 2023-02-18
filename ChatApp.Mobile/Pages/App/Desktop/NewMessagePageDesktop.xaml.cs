namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class NewMessagePageDesktop : ContentPage
{
    private readonly NewMessageViewModel _viewModel;
    public NewMessagePageDesktop(NewMessageViewModel viewModel)
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