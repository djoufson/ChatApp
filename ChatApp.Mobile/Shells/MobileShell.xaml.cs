namespace ChatApp.Mobile.Shells;

public partial class MobileShell : Shell
{
	public MobileShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(InboxPage), typeof(InboxPage));
        Routing.RegisterRoute(nameof(NewMessagePage), typeof(NewMessagePage));
    }
}
