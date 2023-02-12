using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatApp.Api.Hubs;

[Authorize]
public class MessagesHub : Hub
{
    private readonly CacheContext _cacheContext;
    private readonly UserManager<AppUser> _userManager;

    // CONSTRUCTOR
    public MessagesHub(
        CacheContext cacheContext, 
        UserManager<AppUser> userManager)
    {
        _cacheContext = cacheContext;
        _userManager = userManager;
    }

    public async Task SendMessageToUser(string toUserMail, MessageWithoutEntities message)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (issuer is null) return;

        var user = await _cacheContext.Connections
            .Where(c => c.UserEmail == toUserMail)
            .FirstOrDefaultAsync();

        if (user is null) return;
        await Clients
            .Client(user.ConnectionId)
            .SendAsync(EventNames.MessageRecieved, new RecievedMessageEventArg()
            {
                IssuerEmail = issuer.UserEmail,
                IssuerName = issuer.UserName,
                Message = message, 
                Status = true
            });
    }

    public async Task SendMessageToGroup(int groupId, MessageWithoutEntities message)
    {
        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);
        
        if (issuer is null) return;

        // We retrieve the group in the local store
        var group = (await Utils.RetrieveGroup(_cacheContext, groupId));
        if (group is null) return;
        await Clients
            .GroupExcept(group.Id, issuer.ConnectionId)
            .SendAsync(EventNames.MessageRecievedFromGroup, new RecievedMessageEventArg()
            {
                IssuerEmail = issuer.UserEmail,
                IssuerName = issuer.UserName,
                Message = message,
                Status = true
            });
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        var (_, email) = GetUserInfos(Context.User!);
        var user = await _userManager.FindByEmailAsync(email);
        if(user is null) return;
        Console.WriteLine($"---> {email} just joined the chat");
        await _cacheContext.Connections.AddAsync(new ChatUserConnection()
        {
            ConnectionId = Context.ConnectionId,
            UserName = user.UserName,
            UserEmail = email
        });
        await Clients
            .All
            .SendAsync(EventNames.OnlineStatusChanged, new OnlineStatusChangedEventArgs()
            {
                Status = true,
                UserEmail = user.Email,
                UserName = user.UserName,
                Online = true
            });

        await _cacheContext.SaveChangesAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        var connection = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);
        if(connection is null) return;
        Console.WriteLine($"---> {connection.UserEmail} lived the chat right now");
        await Clients
            .All
            .SendAsync(EventNames.OnlineStatusChanged, new OnlineStatusChangedEventArgs()
            {
                Status = true,
                UserEmail = connection.UserEmail,
                UserName = connection.UserName,
                Online = false
            });
        _cacheContext.Connections.Remove(connection);
        await _cacheContext.SaveChangesAsync();
    }
}
