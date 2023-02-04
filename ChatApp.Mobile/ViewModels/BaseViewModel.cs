namespace ChatApp.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly IMessageConnection _chatConnection;
    public static string DeviceTokenRoute => Constants.DEVICE_TOKEN_ROUTE;
    public static string LoginRoute => Constants.LOGIN_ROUTE;
    public static string ConversationsRoute => Constants.CONVERSATIONS_ROUTE;
    public static string AuthToken 
    {
        get => Preferences.Get(Constants.AUTH_TOKEN_KEY, "");
        set => Preferences.Set(Constants.AUTH_TOKEN_KEY, value); 
    }

    public static string DeviceToken
    {
        get => Preferences.Get(Constants.DEVICE_TOKEN_KEY, "");
        set => Preferences.Set(Constants.DEVICE_TOKEN_KEY, value);
    }

    public BaseViewModel()
    {
        var chatConnection = Application.Current.Handler.MauiContext.Services.GetRequiredService<IMessageConnection>();
        _chatConnection = chatConnection;
        _chatConnection.OnMessageRecieved += MessageRecieved;
    }

    protected virtual void MessageRecieved(object sender, string message)
    {
        // When the viewModel Recieves a new message
    }
}