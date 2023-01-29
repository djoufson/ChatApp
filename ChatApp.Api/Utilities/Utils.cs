namespace ChatApp.Api.Utilities;

public static class Utils
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
}
