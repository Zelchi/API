using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Models;

namespace Backend.Server.Tools;

public static class TokenAccount
{
    public static string GenerateJwtToken(AccountEntity account, IConfiguration config)
    {
        var key = config["JWT:Key"] ?? throw new Exception("JWT Key not configured");
        var hours = Convert.ToInt32(config["JWT:ExpireHours"] ?? throw new Exception("JWT Hours is not configured"));
        var expires = DateTime.UtcNow.AddHours(hours);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email.ToString()),
            new Claim(ClaimTypes.Name, account.Username.ToString()),
            new Claim(ClaimTypes.Role, account.Role.ToString())
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}