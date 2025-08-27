using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Server.Config.Services;

public static class Auth
{
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["JWT:Key"] ?? throw new Exception("JWT Key not configured");
        if (key.Length < 32) throw new Exception("JWT Key must be at least 32 characters long");

        var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = true,
            IssuerSigningKey = signingKey
        };

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => { o.TokenValidationParameters = tokenValidationParameters; });
        services.AddAuthorization();
    }
}