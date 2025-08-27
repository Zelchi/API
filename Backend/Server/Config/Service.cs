using Backend.Server.Config.Services;
using Database;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace Backend.Server.Config;

public static class Service
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCors();

        services.ConfigureSwagger(configuration);
        services.ConfigureAuth(configuration);

        services.AddDbContext<Context>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection"));
        });

        // Repositories
        services.AddScoped<Routes.Account.AccountRepository>();
        services.AddScoped<Routes.Product.ProductRepository>();

        // Services
        services.AddScoped<Routes.Account.AccountService>();
        services.AddScoped<Routes.Product.ProductService>();

        return services;
    }
}