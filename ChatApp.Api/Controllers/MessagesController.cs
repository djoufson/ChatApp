namespace ChatApp.Api.Controllers;

[Route("api/[controller]")]
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
    public ActionResult Messages()
    {
        return Ok(1);
    }

    [HttpPost]
    [Route("send/to")]
    public ActionResult SendToSingle()
    {
        return Ok();
    }

    [HttpPost]
    [Route("send/to/group")]
    public ActionResult SendToGroup()
    {
        return Ok();
    }
}
