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
    private readonly IMyHttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GroupController(
        UserManager<AppUser> userManager,
        AppDbContext dbContext,
        IMyHttpClient client,
        IConfiguration configuration,
        IMapper mapper)
    {
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
        _httpClient = client;
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
    [Route("{id}/user")]
    public async Task<ActionResult<IEnumerable<UserWithoutEntities>>> GetUsers(int id)
    {
        var group = await _dbContext
            .Groups
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (group is null) return BadRequest();
        var membersDto = _mapper.Map<ICollection<UserDto>>(group.Members);
        var members = _mapper.Map<ICollection<UserWithoutEntities>>(membersDto);
        return Ok(members);
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

        var usersEmails = groupRequest.MembersMailAddresses;
        var members = await GetUsersByEmailsAsync(usersEmails, _userManager);
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


    [HttpPost]
    [Route("{id}/user")]
    public async Task<ActionResult<GroupWithoutEntities>> AddSingleUserToGroup(int id, EmailDto email)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest();

        var user = await _userManager.FindByEmailAsync(email.Email);
        if (user is null) return BadRequest();

        if(group.Members.Contains(user)) return BadRequest();

        group.Members.Add(user);
        _dbContext.SaveChanges();

        var groupDto = _mapper.Map<GroupDto>(group);
        var responseGroup = _mapper.Map<GroupWithoutEntities>(groupDto);
        return Ok(responseGroup);
    }


    [HttpPost]
    [Route("{id}/user/multiple")]
    public async Task<ActionResult<GroupWithoutEntities>> AddMultipleUsersToGroup(int id, [Required] string[] emails)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest();
        var users = new AppUser[emails.Length];
        for (int i = 0; i < emails.Length; i++)
        {
            users[i] = await _userManager.FindByEmailAsync(emails[i]);
            if (users[i] is null) continue;
            if (group.Members.Contains(users[i])) continue;
            group.Members.Add(users[i]);
        }

        _dbContext.SaveChanges();

        var groupDto = _mapper.Map<GroupDto>(group);
        var responseGroup = _mapper.Map<GroupWithoutEntities>(groupDto);
        return Ok(responseGroup);
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var group = await _dbContext.FindAsync<Group>(id);
        if (group is null) return BadRequest();

        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync();

        return Ok(group);
    }


    [HttpDelete]
    [Route("{id}/user")]
    public async Task<ActionResult> RemoveUserFromGroup(int id, [Required] string email)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest();

        var user = await _userManager.FindByEmailAsync(email);
        group.Members.Remove(user);
        await _dbContext.SaveChangesAsync();

        var groupDto = _mapper.Map<GroupDto>(group);
        var responseGroup = _mapper.Map<GroupWithoutEntities>(groupDto);
        return Ok(responseGroup);
    }
}
