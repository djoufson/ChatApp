namespace ChatApp.Mobile.Utilities;

public class Constants
{
    public const int SSL_PORT = 7177;
    public const int PORT = 5177;
    public const string BASE_URL = "api/v1/";
    public const string IP_ADDRESS = "192.168.180.1"; // The IP Address of my local PC
    public static readonly string FULL_API_BASE_URL = $"http://{IP_ADDRESS}:{PORT}/api/v1/";
    public static string DEV_SERVER_NAME =>
#if ANDROID
        "10.0.2.2";
#else
        "localhost";
#endif

    public static readonly string FULL_URL = $"http://{IP_ADDRESS}:{PORT}";
    public const string AUTH_TOKEN_KEY = "Auth_Token";
    public const string DEVICE_TOKEN_KEY = "Device_Token";
    public const string USER_EMAIL_KEY = "User_Email";
    public const string USERNAME_KEY = "Username";

    // APi Routes
    public const string DEVICE_TOKEN_ROUTE = "account/device-token";
    public const string LOGIN_ROUTE = "account/login";
    public const string CONVERSATIONS_ROUTE = "conversations";
}
