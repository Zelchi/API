using Backend.Server.Routes.Account;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Server.Tools;

public static class TokenGenerator
{
    public static string GenerateJwtToken(AccountEntity account, IConfiguration config)
    {
        var key = config["JWT:Key"] ?? "DefaultSecretKeyThatIsAtLeast32CharactersLong";
        if (key.Length < 32) throw new InvalidOperationException("A chave JWT deve ter pelo menos 32 caracteres");

        var expiration = DateTime.UtcNow.AddHours(config.GetValue<int?>("JWT:ExpirationHours") ?? 8);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email ?? ""),
            new Claim(ClaimTypes.Name, account.Username ?? ""),
            new Claim(ClaimTypes.Role, account.Role ?? "")
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}