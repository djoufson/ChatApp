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

    public async Task DeliverMessage(string issuerMail, int messageId, int conversationId)
    {
        var user = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

        if (user is null) return;

        var issuer = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.UserEmail == issuerMail);

        if (issuer is null) return;
        await Clients.Client(issuer.ConnectionId).SendAsync(EventNames.MessageDelivered, new MessageDeliveredEventArgs()
        {
            Status = true,
            MessageId = messageId,
            UserEmail = user.UserEmail,
            ConversationId = conversationId,
            UserName = user.UserName
        });
    }

    public async Task OpenConversation(int conversationId, string toUserEmail)
    {
        var user = await _cacheContext.Connections
            .FirstOrDefaultAsync(c => c.UserEmail == toUserEmail);

        if (user is null) return;

        await Clients.Client(user.ConnectionId).SendAsync(EventNames.MessageDelivered, new ConversationOpenedEventArgs()
        {
            Status = true,
            ConversationId = conversationId
        });
    }
}
