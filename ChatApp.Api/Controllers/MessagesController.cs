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
    public ActionResult SendToSingle()
    {
        return Ok();
    }

    [HttpPost]
    [Route("group")]
    public ActionResult SendToGroup()
    {
        return Ok();
    }
}
