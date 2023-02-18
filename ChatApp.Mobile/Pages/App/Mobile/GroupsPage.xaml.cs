namespace ChatApp.Mobile.Pages.App.Mobile;

public partial class GroupsPage : ContentPage
{
	public GroupsPage(GroupsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private void ConversationTapped(object sender, ItemTappedEventArgs e)
	{

	}
}