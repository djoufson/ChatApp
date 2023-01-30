using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Api.Utilities.Authentication;

public static class Auth
{
    public static JwtSecurityToken? GetToken(List<Claim> authClaims, IConfiguration configuration)
    {
        var secret = configuration["JWT:Secret"];
        if (secret is null) return null;
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMonths(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public static async Task<JwtSecurityToken?> CreateToken(
        AppUser user,
        UserManager<AppUser> userManager,
        IConfiguration configuration)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = GetToken(authClaims, configuration);
        if (token is null)
            return null;
        return token;

    }

    public static (string userId, string userEmail) GetUserInfos(ClaimsPrincipal User)
    {
        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userid is null || userEmail is null)
            throw new Exception("Unexisting user");

        return (userid, userEmail);
    }

    public static async Task<AppUser[]> GetUsersByEmailsAsync(IEnumerable<string> emails, UserManager<AppUser> userManager)
    {
        var users = new List<AppUser>();
        foreach (var id in emails)
        {
            users.Add(await userManager.FindByEmailAsync(id));
        }
        return users.ToArray();
    }
}
