using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Server.Config;

public static class Authentication
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        string key = configuration.GetSection("JWT").GetValue<string>("Key") ?? "DefaultSecretKeyThatIsAtLeast32CharactersLong";
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}