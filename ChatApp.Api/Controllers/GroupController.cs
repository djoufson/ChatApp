namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
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
    public async Task<ActionResult<BaseResponseDto<IEnumerable<GroupWithoutEntities>>>> GetGroups()
    {
        var groups = await _dbContext
            .Groups
            .Include(g => g.Members)
            .ToArrayAsync();
        var groupsDto = _mapper.Map<GroupDto[]>(groups);
        return Ok(MyOk(_mapper.Map<GroupWithoutEntities[]>(groupsDto)));
    }


    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> GetSingleGroup(int id)
    {
        var groups = await _dbContext
            .Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        return Ok(MyOk(groups?.WithoutEntities(_mapper)));
    }


    [HttpGet]
    [Route("{id:int}/user")]
    public async Task<ActionResult<BaseResponseDto<IEnumerable<BaseResponseDto<UserWithoutEntities>>>>> GetUsers(int id)
    {
        var group = await _dbContext
            .Groups
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (group is null) return BadRequest(MyBadRequest("The requested group does not exist"));
        var membersDto = _mapper.Map<ICollection<UserDto>>(group.Members);
        var members = _mapper.Map<ICollection<UserWithoutEntities>>(membersDto);
        return Ok(MyOk(members, "Success"));
    }


    [HttpPost]
    public async Task<ActionResult<BaseResponseDto<GroupDto>>> CreateGroup(AddGroupDto groupRequest)
    {
        if (groupRequest is null) return BadRequest(MyBadRequest("There are no given parameters"));

        var usersEmails = groupRequest.MembersMailAddresses;
        var members = await GetUsersByEmailsAsync(usersEmails, _userManager);
        var group = new Group()
        {
            Name = groupRequest.Name,
        };
        if (members is null) return BadRequest(MyBadRequest("The requested users to add do not exist"));
        group.Members = new Collection<AppUser>();

        foreach (var member in members)
            group.Members.Add(member);

        _dbContext.Add(group);

        _dbContext.SaveChanges();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }


    [HttpPost]
    [Route("{id:int}/user")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> AddSingleUserToGroup(int id, EmailDto email)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest(MyBadRequest("The requested group does not exist"));

        var user = await _userManager.FindByEmailAsync(email.Email);
        if (user is null) return BadRequest(MyBadRequest("The requested user to add does not exist"));

        if(group.Members.Contains(user)) return BadRequest(MyBadRequest("The requested user to add is already member of this group"));

        group.Members.Add(user);
        _dbContext.SaveChanges();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }


    [HttpPost]
    [Route("{id:int}/user/multiple")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> AddMultipleUsersToGroup(int id, [Required] string[] emails)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest(MyBadRequest("The requested group does not exist"));
        var users = new AppUser[emails.Length];
        for (int i = 0; i < emails.Length; i++)
        {
            users[i] = await _userManager.FindByEmailAsync(emails[i]);
            if (users[i] is null) continue;
            if (group.Members.Contains(users[i])) continue;
            group.Members.Add(users[i]);
        }

        _dbContext.SaveChanges();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }


    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> DeleteGroup(int id)
    {
        var group = await _dbContext.FindAsync<Group>(id);
        if (group is null) return BadRequest(MyBadRequest("The requested group does not exist"));

        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }


    [HttpDelete]
    [Route("{id:int}/user")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> RemoveUserFromGroup(int id, [Required] string email)
    {
        var group = await _dbContext.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null) return BadRequest(MyBadRequest("The requested group does not exist"));

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null) return BadRequest(MyBadRequest("The requested user does not exist"));
        group.Members.Remove(user);
        await _dbContext.SaveChangesAsync();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }
}
