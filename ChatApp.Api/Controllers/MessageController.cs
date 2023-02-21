namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IFirebaseAdmin _firebaseAdmin;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public MessageController(
        UserManager<AppUser> userManager,
        AppDbContext dbContext,
        IConfiguration configuration,
        IFirebaseAdmin firebaseAdmin,
        IMapper mapper)
    {
        _firebaseAdmin = firebaseAdmin;
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageWithoutEntities>>> GetMessages()
    {
        AppUser user = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Bad Request", e.Message)); }
        var messages = await _dbContext.Messages
            .Where(m => m.ToUserId == user.Id || m.FromUserId == user.Id)
            .ToArrayAsync();
        var messagesDto = _mapper.Map<MessageDto[]>(messages);

        return Ok(MyOk(_mapper
            .Map<MessageWithoutEntities[]>(messagesDto)
            .OrderBy(m => m.SentAt)));
    }


    [HttpPost]
    public async Task<ActionResult<MessageWithoutEntities>> SendToSingle(SendMessageDto message)
    {
        AppUser sender = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            sender = await _userManager.FindByEmailAsync(userEmail);
            if (sender is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Bad Request", e.Message)); }
        
        var user = await _userManager.FindByEmailAsync(message.ToUserMail);
        if (user is null) return BadRequest(MyBadRequest("Bad Reques", "The user this message is destined to does not exist"));
        var conversation = await _dbContext.Conversations
            .Include(c => c.Participents)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Participents.Contains(sender) && c.Participents.Contains(user));


        if (conversation is null)
        {
            conversation = new Conversation()
            {
                Messages = new Collection<Message>(),
                Participents = new Collection<AppUser>() { sender, user },
                StartedAt = DateTime.Now
            };
            _dbContext.Conversations.Add(conversation);
        }

        // We increment tu number of unread messages
        conversation.UnreadMessagesCount += 1;
        var messageEntity = new Message()
        {
            FromUserId = sender.Id,
            FromUserEmail = sender.Email,
            FromUserName = sender.UserName,
            ToUserId = user.Id,
            ToUserEmail = user.Email,
            ToUserName = user.UserName,
            Content = message.Content,
            SentAt = DateTime.Now,
            Conversation = conversation
        };

        conversation.Messages ??= new Collection<Message>();
        conversation.Messages.Add(messageEntity);
        var messsageDto = messageEntity.WithoutEntities(_mapper);
        await _dbContext.SaveChangesAsync();
        await _firebaseAdmin.PushAsync(user.DeviceToken!, user.UserName, message.Content, null!);
        return Ok(MyOk(messsageDto));
    }
}
