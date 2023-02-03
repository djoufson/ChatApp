namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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
            return BadRequest(MyBadRequest("The requested user does not exist"));

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginInfos.Password);
        
        if (passwordCheck)
        {
            var token = await CreateToken(user, _userManager, _configuration);
            if (token is null) return Unauthorized();
            return Ok(
                new LoginResponseDto()
                {
                    User = user.WithoutEntities(_mapper),
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
    public async Task<ActionResult<BaseResponseDto<UserWithoutEntities>>> Register(RegisterDto registerInfos)
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
            return BadRequest(MyBadRequest(
                "User creation failed! Please check user details and try again.",
                result.Errors.Select(e => e.Description).ToArray()));
        }

        return Ok(MyOk(user.WithoutEntities(_mapper)));
    }


    [HttpPost]
    [Route("logout")]
    [Authorize]
    public async Task<ActionResult<BaseResponseDto<UserWithoutEntities>>> Logout(LogoutDto logoutInfos)
    {
        try
        {
            var (uId, uEmail) = GetUserInfos(User);
            var user = await _userManager.FindByIdAsync(uId);
            if(user is null) return Unauthorized(new BaseResponseDto
            {
                Message = "Unauthorized",
                Status = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Errors = new[] { "You are not authorized" }
            });
            var passwordCheck = await _userManager.CheckPasswordAsync(user, logoutInfos.Password);
            if (!passwordCheck) return BadRequest(MyBadRequest(
                "Wong password",
                "The password you entered is not correct"));
            
            return Ok(MyOk(user.WithoutEntities(_mapper)));
        }
        catch (Exception e)
        {
            return BadRequest(MyBadRequest(e.Message, e.Message));
        }
    }
}
