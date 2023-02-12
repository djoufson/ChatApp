namespace ChatApp.Api.Hubs;

public class OnlineStatusHub : Hub
{
    private readonly CacheContext _cacheContext;

    // CONSTRUCTOR
    public OnlineStatusHub(
        CacheContext cacheContext)
    {
        _cacheContext = cacheContext;
    }

    public async Task ChangeOnlineStatus(bool status)
    {
        var user = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);
        if (user is null) return;

        await Clients
            .All
            .SendAsync(EventNames.OnlineStatusChanged, new OnlineStatusChangedEventArgs()
            {
                Status = true,
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                Online = status
            });
    }
}
