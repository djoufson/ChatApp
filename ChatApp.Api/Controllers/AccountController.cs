using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly UserManager<AppUser> _userManager;
	private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AccountController(
		UserManager<AppUser> userManager, 
		AppDbContext dbContext,
        IConfiguration configuration)
	{
		_userManager = userManager;
		_dbContext = dbContext;
        _configuration = configuration;
	}

	[HttpPost]
	[Route("login")]
	public async Task<ActionResult> Login(LoginDto loginInfos)
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
    [Route("logout")]
    [Authorize]
    public ActionResult Logout(string password)
    {
        return Ok(1);
    }

    [HttpPost]
    [Route("register")]
    public ActionResult Register()
    {
        return Ok(1);
    }
}
