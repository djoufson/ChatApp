namespace ChatApp.Mobile.Shells;

public partial class DesktopShell : Shell
{
	public DesktopShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(InboxPage), typeof(InboxPageDesktop));
        Routing.RegisterRoute(nameof(NewMessagePage), typeof(NewMessagePageDesktop));
    }
}