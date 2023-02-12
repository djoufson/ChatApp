namespace ChatApp.Api.Hubs;

public class MessageStatusHub : Hub
{
    private readonly CacheContext _cacheContext;
    private readonly UserManager<AppUser> _userManager;

    public MessageStatusHub(
        CacheContext cacheContext,
        UserManager<AppUser> userManager)
    {
        _cacheContext = cacheContext;
        _userManager = userManager;
    }

    public Task DeliverMessage(string groupId)
    {
        throw new NotImplementedException();
    }

    public Task OpenMessage(string groupId)
    {
        throw new NotImplementedException();
    }
}
