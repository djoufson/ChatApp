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

        var conversationsDto = _mapper.Map<ConversationDto[]>(conversations);
        return Ok(MyOk(_mapper.Map<ConversationWithoutEntities[]>(conversationsDto)));
    }

    [HttpGet]
    [Route("{id:int}/messages")]
    public async Task<ActionResult> GetMessagesFromACOnversation(int id)
    {
        var conversation = await _dbContext.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id);
        var messagesDto = _mapper.Map<MessageDto[]>(conversation?.Messages);
        
        return Ok(MyOk(_mapper.Map<MessageWithoutEntities[]>(messagesDto)));
    }
}
