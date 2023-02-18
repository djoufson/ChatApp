namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class HomePageDesktop : ContentPage
{
    private readonly HomeViewModel _viewModel;
    public HomePageDesktop(HomeViewModel viewModel)
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