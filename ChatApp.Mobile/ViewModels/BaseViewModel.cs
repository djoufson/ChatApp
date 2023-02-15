namespace ChatApp.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    private readonly IGroupConnection _groupConnection;
    private readonly IMessageConnection _messageConnection;
    private readonly IOnlineStatusConnection _onlineStatusConnection;
    private readonly IMessageStatusConnection _messageStatusConnection;

    [ObservableProperty] private bool _isBusy;
    public static string LoginRoute => Constants.LOGIN_ROUTE;
    public static string DeviceTokenRoute => Constants.DEVICE_TOKEN_ROUTE;
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
    public BaseViewModel()
    {
        // Construction without chat connection
    }

    // CONSTRUCTOR
    public BaseViewModel(
        IGroupConnection groupConnection,
        IMessageConnection messageConnection,
        IOnlineStatusConnection onlineStatusConnection,
        IMessageStatusConnection messageStatusConnection)
    {
        // Construction with chat connection
        _groupConnection = groupConnection;
        _messageConnection = messageConnection;
        _onlineStatusConnection = onlineStatusConnection;
        _messageStatusConnection = messageStatusConnection;

        _messageConnection.OnMessageRecieved += OnMessageRecieved;
        _groupConnection.OnGroupJoined += OnGroupJoined;
        _groupConnection.OnGroupLeft += OnGroupLeft;
        _groupConnection.OnRemovedFromGroup += OnRemovedFromGroup;
        _messageStatusConnection.OnConversationOpened += OnConversationOpened;
        _messageStatusConnection.OnMessageDelivered += OnMessageDelivered;
        _onlineStatusConnection.OnlineStatusChanged += OnlineStatusChanged;
    }

    protected virtual void OnlineStatusChanged(object sender, OnlineStatusChangedEventArgs e)
    {
        if (_onlineStatusConnection is null) 
            throw new NullReferenceException("The class implementation is not provided an onlineStatus Connection");
    }

    protected virtual void OnMessageDelivered(object sender, MessageDeliveredEventArgs e)
    {
        if (_messageStatusConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a messageStatus Connection");
    }

    protected virtual void OnConversationOpened(object sender, ConversationOpenedEventArgs e)
    {
        if (_messageStatusConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a messageStatus Connection");
    }

    protected virtual void OnRemovedFromGroup(object sender, RemovedFromGroupEventArgs e)
    {
        if (_groupConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a group Connection");
    }

    protected virtual void OnGroupLeft(object sender, GroupLeftEventArgs e)
    {
        if (_groupConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a group Connection");
    }

    protected virtual void OnGroupJoined(object sender, GroupJoinedEventArg e)
    {
        if (_groupConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a group Connection");
    }

    protected virtual void OnMessageRecieved(object sender, RecievedMessageEventArg e)
    {
        if(_messageConnection is null) 
            throw new NullReferenceException("The class implementation is not provided a chat connection");
    }
}