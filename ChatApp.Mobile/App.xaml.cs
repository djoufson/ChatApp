namespace ChatApp.Mobile;

public partial class App : Application
{
    public static string AuthToken => Preferences.Get(Constants.AUTH_TOKEN_KEY, "");
	public static string UserEmail 
	{
		get => Preferences.Get(Constants.USER_EMAIL_KEY, "");
		set => Preferences.Set(Constants.USER_EMAIL_KEY, value); 
	}
    public static string UserName
    {
        get => Preferences.Get(Constants.USERNAME_KEY, "");
        set => Preferences.Set(Constants.USERNAME_KEY, value);
    }

    public App()
	{
		InitializeComponent();
		var isDesktop = DeviceProperties.IsDesktop;

        MainPage = isDesktop ? 
			new DesktopShell() : 
			new MobileShell();
	}
}
