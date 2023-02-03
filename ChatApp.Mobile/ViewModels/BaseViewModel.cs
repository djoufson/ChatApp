namespace ChatApp.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly IMessageConnection _chatConnection;
    public static string AuthToken 
    {
        get => Preferences.Get(Constants.AuthTokenKey, "");
        set => Preferences.Set(Constants.AuthTokenKey, value); 
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