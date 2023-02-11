using ChatApp.Shared.Utilities;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs;

[Authorize]
public class MessagesHub : Hub
{
    private readonly CacheContext _cacheContext;
    private readonly UserManager<AppUser> _userManager;

    public MessagesHub(CacheContext cacheContext, UserManager<AppUser> userManager)
    {
        _cacheContext = cacheContext;
        _userManager = userManager;
    }
    public async Task SendMessage(string message, string toUserMail)
    {
        var connectionId = await _cacheContext.Connections
            .Where(c => c.UserEmail == toUserMail)
            .Select(c => c.ConnectionId)
            .FirstOrDefaultAsync();
        if (connectionId is null) return;
        await Clients.Client(connectionId).SendAsync(EventNames.MessageRecieved, message);
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
            UserEmail = email
        });
        _cacheContext.SaveChanges();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        var connection = await _cacheContext.Connections.FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);
        if(connection is null) return;
        Console.WriteLine($"---> {connection.UserEmail} lived the chat right now");
        _cacheContext.Connections.Remove(connection);
        _cacheContext.SaveChanges();
    }
}
