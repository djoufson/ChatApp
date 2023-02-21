namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController, Authorize]
public class ConversationsController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public ConversationsController(
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
    public async Task<ActionResult<IEnumerable<ConversationWithoutEntities>>> GetAllCOnversations()
    {
        AppUser user = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Unauthorized", e.Message)); }

        var conversations = await _dbContext
            .Conversations
            .Include(c => c.Participents)
            .Include(c => c.Messages)
            .Where(c => c.Participents.Contains(user)).ToArrayAsync();

        conversations = conversations
            .Sort();
        var conversationsDto = _mapper.Map<ConversationDto[]>(conversations);
        return Ok(MyOk(_mapper.Map<ConversationWithoutEntities[]>(conversationsDto)));
    }


    [HttpGet]
    [Route("messages")]
    public async Task<ActionResult<IEnumerable<ConversationWithoutEntities>>> GetMessagesFromACOnversation(string userEmail)
    {
        AppUser user = null!;
        try
        {
            var (uId, uEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(uEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Unauthorized", e.Message)); }
        var user2 = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user2 is null) return BadRequest(MyBadRequest("The user you want to send a message to does no longer exist"));

        var conversation = await _dbContext
            .Conversations
            .Include(c => c.Participents)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Participents.Contains(user) && c.Participents.Contains(user2));

        var messagesDto = _mapper.Map<MessageDto[]>(conversation?.Messages);

        return Ok(MyOk(_mapper.Map<MessageWithoutEntities[]>(messagesDto)));
    }


    [HttpGet]
    [Route("{id:int}/messages")]
    public async Task<ActionResult> GetMessagesFromACOnversation(int id)
    {
        var conversation = await _dbContext.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id);
        var messagesDto = _mapper.Map<MessageDto[]>(conversation?.Messages);
        
        return Ok(MyOk(_mapper.Map<MessageWithoutEntities[]>(messagesDto)));
    }


    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult> UpdateUnreadMessagesCount(int id)
    {
        // Retrieve the Conversation
        var conversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == id);

        if(conversation is null) return BadRequest(MyBadRequest("Bad Request", "The specified conversation id does not exist"));
        conversation.UnreadMessagesCount = 0;
        await _dbContext.SaveChangesAsync();
        return Ok(MyOk("Updated Succcesfully"));
    } 
}
