namespace ChatApp.Mobile.Pages.App.Desktop;

public partial class GroupsPageDesktop : ContentPage
{
	public GroupsPageDesktop(
        GroupsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private void ConversationTapped(object sender, ItemTappedEventArgs e)
	{

	}
}