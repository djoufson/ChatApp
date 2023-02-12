namespace ChatApp.Api.Hubs;

[Authorize]
public class GroupHub : Hub
{
    private readonly CacheContext _cacheContext;

    // CONSTRUCTOR
    public GroupHub(
        CacheContext cacheContext)
    {
        _cacheContext = cacheContext;
    }

    public async Task RestoreGroupConnection(int groupId)
    {
        var userConnectionId = Context.ConnectionId;
        if (string.IsNullOrEmpty(userConnectionId)) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;
        await Groups.AddToGroupAsync(userConnectionId, group.Id);
    }

    public async Task JoinGroup(int groupId)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (issuer is null) return;
        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Clients
            .Group(group.Id)
            .SendAsync(EventNames.GroupJoined, new GroupJoinedEventArg()
            {
                GroupId = groupId,
                Status = true,
                UserEmail = issuer.UserEmail,
                UserName = issuer.UserName,
            });

        await Groups.AddToGroupAsync(issuer.ConnectionId, group.Id);
    }

    public async Task AddToGroup(int groupId, string email)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (issuer is null) return;
        // We will next check if this user has appropriete Role to Add or remove Users
        var user = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.UserEmail == email);
        if (user is null) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Groups.AddToGroupAsync(user.ConnectionId, group.Id);

        await Clients
            .GroupExcept(group.Id, issuer.ConnectionId)
            .SendAsync(EventNames.AddedToGroup, new AddedToGroupEventArgs()
            {
                Status = true,
                GroupId = groupId,
                IssuerEmail = issuer.UserEmail,
                IssuerName = issuer.UserName,
                NewUserEmail = user.UserEmail,
                NewUserName = user.UserName,
            });
    }

    public async Task RemoveFromGroup(int groupId, string email)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (issuer is null) return;

        // We will next check if this user has appropriete Role to Add or remove Users
        var user = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.UserEmail == email);
        if (user is null) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId))!;

        await Clients
            .GroupExcept(group.Id, issuer.ConnectionId)
            .SendAsync(EventNames.RemovedFromGroup, new RemovedFromGroupEventArgs()
            {
                Status = true,
                GroupId = groupId,
                IssuerEmail = issuer.UserEmail,
                IssuerName = issuer.UserName,
                RemovedUserEmail = user.UserEmail,
                RemovedUserName = user.UserName,
            });

        await Groups.RemoveFromGroupAsync(user.ConnectionId, group.Id);
    }

    public async Task LeaveGroup(int groupId)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (issuer is null) return;

        var userConnectionId = Context.ConnectionId;

        // We retrieve the group in the local store
        var group = await Utils.RetrieveGroup(_cacheContext, groupId, true);
        if (group is null)
            return;

        await Groups.RemoveFromGroupAsync(userConnectionId, group.Id);
        await Clients
            .Group(group.Id)
            .SendAsync(EventNames.GroupLeft, new GroupLeftEventArgs() 
            {
                Status = true,
                GroupId = groupId,
                UserEmail = issuer.UserEmail,
                UserName = issuer.UserName
            });
    }
}
