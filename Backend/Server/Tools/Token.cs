using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Models;

namespace Backend.Server.Tools;

public static class TokenGenerator
{
    public static string GenerateJwtToken(AccountEntity account, IConfiguration settings)
    {
        var key = settings["JWT:Key"] ?? throw new Exception("JWT Key not configured");
        var hours = Convert.ToInt32(settings["JWT:ExpirationHours"] ?? throw new Exception("JWT ExpirationHours is not configured"));
        var expires = DateTime.UtcNow.AddHours(hours);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Role, account.Role)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}