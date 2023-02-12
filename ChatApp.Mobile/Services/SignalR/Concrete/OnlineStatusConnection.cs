namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class OnlineStatusConnection : IOnlineStatusConnection
{
    private readonly HubConnection _connection;

    public event EventHandler<OnlineStatusChangedEventArgs> OnlineStatusChanged;

    // CONSTRUCTOR
    public OnlineStatusConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7177/{HubRoutes.OnlineStatusRoute}")
            .Build();

        Task.Run(async () => await ConnectAsync());
        _connection.On<OnlineStatusChangedEventArgs>(EventNames.ChangeOnlineStatus, ChangeOnlineStatus);
    }

    #region EVENT HANDLERS
    private void ChangeOnlineStatus(OnlineStatusChangedEventArgs e)
    {
        OnlineStatusChanged?.Invoke(this, e);
    }
    #endregion

    #region PUBLIC INTERFACE
    public Task ChangeOnlineStatusAsync(bool status)
    {
        return _connection.InvokeAsync(EventNames.ChangeOnlineStatus, status);
    }

    public Task ConnectAsync()
    {
        return _connection.StartAsync();
    }

    public Task DisconnectAsync()
    {
        return _connection.StopAsync();
    }
    #endregion
}
