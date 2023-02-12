namespace ChatApp.Api.Hubs;

[Authorize]
public class GroupHub : Hub
{
    private readonly CacheContext _cacheContext;
    private readonly UserManager<AppUser> _userManager;

    public GroupHub(
        CacheContext cacheContext,
        UserManager<AppUser> userManager)
    {
        _cacheContext = cacheContext;
        _userManager = userManager;
    }

    public async Task RestoreGroup(string groupId)
    {
        var userConnectionId = Context.ConnectionId;
        if (string.IsNullOrEmpty(userConnectionId)) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;
        await Groups.AddToGroupAsync(userConnectionId, group.Id);
    }

    public async Task JoinGroup(string groupId)
    {
        var userConnectionId = Context.ConnectionId;
        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Groups.AddToGroupAsync(userConnectionId, group.Id);
    }

    public async Task AddToGroup(string groupId, string email)
    {
        // We will next check if this user has appropriete Role to Add or remove Users
        var user = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.UserEmail == email);
        if (user is null) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Groups.AddToGroupAsync(user.ConnectionId, group.Id);
    }

    public async Task RemoveFromGroup(string groupId, string email)
    {
        // We will next check if this user has appropriete Role to Add or remove Users
        var user = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.UserEmail == email);
        if (user is null) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Groups.RemoveFromGroupAsync(user.ConnectionId, group.Id);
    }

    public async Task LeaveGroup(string groupId)
    {
        var userConnectionId = Context.ConnectionId;

        // We retrieve the group in the local store
        var group = await Utils.RetrieveGroup(_cacheContext, groupId, true);
        if (group is null)
            return;

        await Groups.RemoveFromGroupAsync(userConnectionId, group.Id);
    }
}
