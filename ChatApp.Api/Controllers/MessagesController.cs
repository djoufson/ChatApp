namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public MessagesController(
        UserManager<AppUser> userManager,
        AppDbContext dbContext,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/{id:int}")]
    public ActionResult GetMessagesFromAGroup()
    {
        return Ok(1);
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
                Participents = new Collection<AppUser>() { sender, user }
            };
            _dbContext.Conversations.Add(conversation);
        }
        var messageEntity = new Message()
        {
            FromUserId = sender.Id,
            ToUserId = user.Id,
            Content = message.Content,
            SentAt = DateTime.Now,
            Conversation = conversation
        };

        conversation.Messages ??= new Collection<Message>();
        conversation.Messages.Add(messageEntity);
        var messsageDto = messageEntity.WithoutEntities(_mapper);
        _dbContext.SaveChanges();
        return Ok(MyOk(messsageDto));
    }

    [HttpPost]
    [Route("group")]
    public ActionResult SendToGroup()
    {
        return Ok();
    }
}
