namespace ChatApp.Mobile.Utilities;

public class Constants
{
    public const int PORT = 7177;
    public const string BASE_URL = "api/v1/";
    public static readonly string FULL_BASE_URL = $"https://localhost:{PORT}api/v1/";
    public static string DEV_SERVER_NAME =>
#if WINDOWS
        "localhost";
#elif ANDROID
        "10.0.2.2";
#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif

    public const string AUTH_TOKEN_KEY = "Auth_Token";
    public const string DEVICE_TOKEN_KEY = "Device_Token";
    public const string USER_EMAIL_KEY = "User_Email";
    public const string USERNAME_KEY = "Username";

    // APi Routes
    public const string DEVICE_TOKEN_ROUTE = "account/device-token";
    public const string LOGIN_ROUTE = "account/login";
    public const string CONVERSATIONS_ROUTE = "conversations";
}
