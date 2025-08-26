using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Server.Config;

public static class Auth
{
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["JWT:Key"] ?? "DefaultSecretKeyThatIsAtLeast32CharactersLong";
        var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            IssuerSigningKey = signingKey
        };

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        });
    }
}