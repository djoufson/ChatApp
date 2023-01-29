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
	public async Task<ActionResult<UserDto>> Login(LoginDto loginInfos)
	{
        var user = await _userManager.FindByEmailAsync(loginInfos.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginInfos.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims, _configuration);
            if (token is null)
                return Unauthorized();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }


    [HttpPost]
    [ValidateModel]
    [Route("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerInfos)
    {
        var userExists = await _userManager.FindByEmailAsync(registerInfos.Email);
        if (userExists is not null)
            return StatusCode(StatusCodes.Status403Forbidden, 
                new BaseResponseDto { Status = false, Message = "User already exists!", StatusCode=0 });

        // Mapping the user according to the registration
        var user = _mapper.Map<AppUser>(registerInfos);

        var result = await _userManager.CreateAsync(user, registerInfos.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseDto { Status = false, Message = "User creation failed! Please check user details and try again.", StatusCode = 500 });

        return Ok(_mapper.Map<UserDto>(user));
    }


    [HttpPost]
    [Route("logout")]
    [Authorize]
    public ActionResult Logout(string password)
    {
        return Ok(1);
    }
}
