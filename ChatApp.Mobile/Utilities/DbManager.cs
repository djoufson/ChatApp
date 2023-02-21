namespace ChatApp.Mobile.Utilities;

public class DbManager
{
    public DbManager()
    {

    }
    public static void ClearPreferences()
    {
        Preferences.Remove(Constants.AUTH_TOKEN_KEY);
        Preferences.Remove(Constants.USER_EMAIL_KEY);
        Preferences.Remove(Constants.USERNAME_KEY);
    }
}
