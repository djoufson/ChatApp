namespace ChatApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public GroupController(
        UserManager<AppUser> userManager,
        AppDbContext dbContext,
        IConfiguration configuration,
        IMapper mapper)
    {
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("{id}/users")]
    public ActionResult GetUsers(int id)
    {
        return Ok(id);
    }

    [HttpPost]
    public ActionResult CreateGroup(GroupDto group)
    {

        return Ok();
    }

    [HttpPost]
    [Route("add-user")]
    public ActionResult AddSingleUserToGroup()
    {
        return Ok();
    }

    [HttpPost]
    [Route("add-user/multiple")]
    public ActionResult AddMultipleUsersToGroup()
    {
        return Ok();
    }
}
