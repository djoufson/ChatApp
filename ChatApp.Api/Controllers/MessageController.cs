﻿namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public MessageController(
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
                Participents = new Collection<AppUser>() { sender, user }
            };
            _dbContext.Conversations.Add(conversation);
        }
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
        _dbContext.SaveChanges();
        return Ok(MyOk(messsageDto));
    }

    [HttpPost]
    [Route("group/{id:int}")]
    public async Task<ActionResult<MessageWithoutEntities>> SendToGroup([Required] int id, SendMessageToGroupDto message)
    {
        AppUser user = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Unauthorized", e.Message)); }

        var group = await _dbContext
            .Groups
            .Include(g => g.Members)
            .Include(g => g.Messages)
            .FirstOrDefaultAsync(g => g.Id == id);

        if(group is null) { return BadRequest(MyBadRequest("Bad Request", "The supplied group does not exist")); }

        if(!group.Members.Contains(user)) return Unauthorized(MyBadRequest("Unauthorized", "You are not a member of this group"));

        var messageEntity = new Message
        {
            FromUserEmail = user.Email,
            FromUserName = user.UserName,
            Content = message.Content,
            Group = group,
            GroupId = group.Id,
            FromUserId = user.Id,
            SentAt = DateTime.Now
        };

        group.Messages ??= new Collection<Message>();
        group.Messages.Add(messageEntity);
        await _dbContext.SaveChangesAsync();
        return Ok(MyOk(messageEntity.WithoutEntities(_mapper), Message: "Message sent successfuly"));
    }
}