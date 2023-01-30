using System.Collections.ObjectModel;

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
    public async Task<ActionResult<GroupDto>> GetGroups()
    {
        var groups = await _dbContext
            .Groups
            .Include(g => g.Members)
            .ToArrayAsync();
        var groupsDto = _mapper.Map<GroupDto[]>(groups);
        return Ok(_mapper.Map<GroupWithoutEntities[]>(groupsDto));
    }

    [HttpGet]
    [Route("{id}/users")]
    public ActionResult GetUsers(int id)
    {
        return Ok(id);
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroup(AddGroupDto groupRequest)
    {
        if (groupRequest is null) return BadRequest(new BaseResponseDto
        {
            Status = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "Bad Request"
        });

        var usersIds = groupRequest.MembersMailAddresses;
        var members = await GetUsersByIdsAsync(usersIds);
        var group = new Group()
        {
            Name = groupRequest.Name,
        };
        if (members is null) return BadRequest();
        group.Members = new Collection<AppUser>();

        foreach (var member in members)
            group.Members.Add(member);

        _dbContext.Add(group);

        _dbContext.SaveChanges();
        var groupDto = _mapper.Map<GroupDto>(group);

        return Ok(_mapper.Map<GroupWithoutEntities>(groupDto));
    }

    async Task<AppUser[]> GetUsersByIdsAsync(IEnumerable<string> ids)
    {
        var users = new List<AppUser>();
        foreach (var id in ids)
        {
            users.Add(await _userManager.FindByEmailAsync(id));
        }
        return users.ToArray();
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

    [HttpDelete]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var group = await _dbContext.FindAsync<Group>(id);
        if (group is null) return BadRequest();

        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync();

        return Ok(group);
    }
}
