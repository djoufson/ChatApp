namespace ChatApp.Mobile.Services.SignalR.Concrete;

public class GroupConnection : IGroupConnection
{
    public event EventHandler<GroupJoinedEventArg> OnGroupJoined;
    public event EventHandler<AddedToGroupEventArgs> OnAddedToGroupEventArgs;
    public event EventHandler<RemovedFromGroupEventArgs> OnRemovedFromGroup;
    public event EventHandler<GroupLeftEventArgs> OnGroupLeft;
    private readonly HubConnection _connection;

    // CONSTRUCTOR
    public GroupConnection()
    {
        _connection = new HubConnectionBuilder()
			.WithUrl($"https://localhost:7177/{HubRoutes.GroupsRoute}", (options) =>
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
        _connection.On<GroupJoinedEventArg>(EventNames.GroupJoined, GroupJoined);
        _connection.On<AddedToGroupEventArgs>(EventNames.AddedToGroup, AddedToGroup);
        _connection.On<RemovedFromGroupEventArgs>(EventNames.RemovedFromGroup, RemovedFromGroup);
        _connection.On<GroupLeftEventArgs>(EventNames.GroupLeft, GroupLeft);
    }

    #region EVENT HANDLERS
    private void GroupJoined(GroupJoinedEventArg e)
    {
        OnGroupJoined?.Invoke(this, e);
    }

    private void AddedToGroup(AddedToGroupEventArgs e)
    {
        OnAddedToGroupEventArgs?.Invoke(this, e);
    }

    private void RemovedFromGroup(RemovedFromGroupEventArgs e)
    {
        OnRemovedFromGroup?.Invoke(this, e);
    }

    private void GroupLeft(GroupLeftEventArgs e)
    {
        OnGroupLeft?.Invoke(this, e);
    }

    #endregion

    #region PUBLIC INTERFACE
    public Task AddToGroupAsync(int groupId, string email)
    {
        return _connection.InvokeCoreAsync(EventNames.AddToGroup, new object[] { groupId, email });
    }

    public Task JoinGroupAsync(int groupId)
    {
        return _connection.InvokeCoreAsync(EventNames.JoinGroup, new object[] { groupId });
    }

    public Task LeaveGroupAsync(int groupId)
    {
        return _connection.InvokeCoreAsync(EventNames.AddToGroup, new object[] { groupId });
    }

    public Task RemoveFromGroupAsync(int groupId, string email)
    {
        return _connection.InvokeCoreAsync(EventNames.RemoveFromGroup, new object[] { groupId });
    }

    public Task RestoreGroupConnectionAsync(int groupId)
    {
        return _connection.InvokeCoreAsync(EventNames.RestoreGroupConnection, new object[] { groupId });
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
