namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class OnlineStatusConnection : IOnlineStatusConnection
{
    private readonly HubConnection _connection;

    public event EventHandler<OnlineStatusChangedEventArgs> OnlineStatusChanged;

    // CONSTRUCTOR
    public OnlineStatusConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{Constants.FULL_URL}/{HubRoutes.OnlineStatusRoute}", (options) =>
            {
                options.Headers.Add("access_token", App.AuthToken);
            })
            .Build();

        Task.Run(() =>
        {
            App.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                try
                {
                    await ConnectAsync();
                }
                catch (Exception) { }
            });
        });
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
