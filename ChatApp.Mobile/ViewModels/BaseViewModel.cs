using ChatApp.Shared.Utilities.EventArgs;

namespace ChatApp.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly IMessageConnection _chatConnection;
    [ObservableProperty] private bool _isBusy;
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

    // CONSTRUCTOR
    public BaseViewModel(
        IMessageConnection chatConnection)
    {
        // Construction with chat connection
        _chatConnection = chatConnection;
        _chatConnection.OnMessageRecieved += MessageRecieved;
    }

    public BaseViewModel()
    {
        // Construction without chat connection
    }

    protected virtual void MessageRecieved(object sender, RecievedMessageEventArg e)
    {
        // When the viewModel Recieves a new message
        if(_chatConnection is null) throw new NullReferenceException("The class implementation is not provided a chat connection");
    }
}