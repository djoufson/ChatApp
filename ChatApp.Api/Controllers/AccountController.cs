namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
	private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AccountController(
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

	[HttpPost]
	[Route("login")]
    [ValidateModel]
	public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginInfos)
	{
        var user = await _userManager.FindByEmailAsync(loginInfos.Email);
        if (user is null) 
            return BadRequest(new BaseResponseDto() 
            { 
                Status=false, 
                StatusCode=StatusCodes.Status400BadRequest, 
                Message = "The requested user does not exist"
            });

        if(await _userManager.CheckPasswordAsync(user, loginInfos.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims, _configuration);
            if (token is null)
                return Unauthorized();

            return Ok(
                new LoginResponseDto()
                {
                    User = _mapper.Map<UserDto>(user),
                    Status = true,
                    StatusCode = StatusCodes.Status200OK,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
        }
        return Unauthorized();
    }


    [HttpPost]
    [ValidateModel]
    [Route("register")]
    public async Task<ActionResult<BaseResponseDto<UserDto>>> Register(RegisterDto registerInfos)
    {
        var userExists = await _userManager.FindByEmailAsync(registerInfos.Email);
        if (userExists is not null)
            return StatusCode(StatusCodes.Status403Forbidden, 
                new BaseResponseDto { Status = false, Message = $"User {registerInfos.Email} already exists!", StatusCode=0 });

        // Mapping the user according to the registration
        var user = _mapper.Map<AppUser>(registerInfos);

        var result = await _userManager.CreateAsync(user, registerInfos.Password);
        if (!result.Succeeded)
        {
            var badRequestContent = new BaseResponseDto()
            {
                Status = false,
                Errors = result.Errors.Select(e => e.Description).ToArray(),
                Message = "User creation failed! Please check user details and try again.",
                StatusCode = StatusCodes.Status400BadRequest,
            };

            return StatusCode(StatusCodes.Status400BadRequest, badRequestContent);
        }

        return Ok(new BaseResponseDto<UserDto>()
        {
            Data = _mapper.Map<UserDto>(user),
            Status = true,
            StatusCode = StatusCodes.Status200OK
        });
    }


    [HttpPost]
    [Route("logout")]
    [Authorize]
    public async Task<ActionResult> Logout(LogoutDto logoutInfos)
    {
        var userid = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        var user = await _userManager.FindByIdAsync(userid);
        if(user is null) return Unauthorized(new BaseResponseDto
        {
            Message = "Unauthorized",
            Status = false,
            StatusCode = StatusCodes.Status401Unauthorized,
            Errors = new[] { "You are not authorized" }
        });
        var passwordCheck = await _userManager.CheckPasswordAsync(user, logoutInfos.Password);
        if (!passwordCheck) return BadRequest(new BaseResponseDto()
        {
            Message = "Wong password",
            Status = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = new[] {"The password you entered is not correct"}
        });
        return Ok(_mapper.Map<UserDto>(user));
    }
}
