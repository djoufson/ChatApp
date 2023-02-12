namespace ChatApp.Api.Hubs;

public class OnlineStatusHub : Hub
{
    private readonly CacheContext _cacheContext;
    private readonly UserManager<AppUser> _userManager;

    public OnlineStatusHub(
        CacheContext cacheContext,
        UserManager<AppUser> userManager)
    {
        _cacheContext = cacheContext;
        _userManager = userManager;
    }

    public Task ChangeOnlineStatus(string groupId)
    {
        throw new NotImplementedException();
    }
}
