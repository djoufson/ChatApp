namespace ChatApp.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _dbContext;

    public GroupController(
        UserManager<AppUser> userManager,
        AppDbContext dbContext,
        IMapper mapper)
    {
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
    }


    [HttpGet]
    public async Task<ActionResult<BaseResponseDto<IEnumerable<GroupWithoutEntities>>>> GetGroups()
    {
        AppUser user = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Unauthorized", e.Message)); }

        var groups = await _dbContext
            .Groups
            .Include(g => g.Members)
            .Include(g => g.Messages)
            .Where(g => g.Members.Contains(user))
            .ToArrayAsync();
        var groupsDto = _mapper.Map<GroupDto[]>(groups);
        return Ok(MyOk(_mapper.Map<GroupWithoutEntities[]>(groupsDto)));
    }


    [HttpGet]
    [Route("{id:int}/message")]
    public async Task<ActionResult<BaseResponseDto<MessageWithoutEntities[]>>> GetSingleGroupMessages(int id)
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
            .FirstOrDefaultAsync(g => g.Id == id && g.Members.Contains(user));

        if(group is null)
        return NotFound(MyBadRequest("Group not Found", "The group you requested does not exist"));
        var messages = group?.Messages;
        var messagesDto = _mapper.Map<MessageDto[]>(messages);

        return Ok(MyOk(_mapper.Map<MessageWithoutEntities[]>(messagesDto)));
    }

    [HttpGet]
    [Route("{id:int}/user")]
    public async Task<ActionResult<BaseResponseDto<IEnumerable<UserWithoutEntities>>>> GetUsers(int id)
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
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> CreateGroup(AddGroupDto groupRequest)
    {
        AppUser user = null!;
        try
        {
            var (userId, userEmail) = GetUserInfos(User);
            user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return Unauthorized();
        }
        catch (Exception e) { return Unauthorized(MyBadRequest("Unauthorized", e.Message)); }
        var usersEmails = groupRequest.MembersMailAddresses;
        var members = await GetUsersByEmailsAsync(usersEmails, _userManager);
        var group = new Group()
        {
            Name = groupRequest.Name
        };
        if (members is null) return BadRequest(MyBadRequest("The requested users to add do not exist"));
        group.Members = new Collection<AppUser>();
        group.Members.Add(user);

        foreach (var member in members)
        {
            if(!group.Members.Contains(member))
                group.Members.Add(member);
        }

        await _dbContext.AddAsync(group);

        await _dbContext.SaveChangesAsync();

        return Ok(MyOk(group.WithoutEntities(_mapper)));
    }


    [HttpPost]
    [Route("{id:int}/user")]
    public async Task<ActionResult<BaseResponseDto<GroupWithoutEntities>>> AddUsersToGroup(int id, [Required] string[] emails)
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

        await _dbContext.SaveChangesAsync();

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


    [HttpPost]
    [Route("{id:int}/message")]
    public async Task<ActionResult> SendMessage([Required] int id, SendMessageToGroupDto message)
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
