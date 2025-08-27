using Backend.Server.Config.Services;
using Database;

namespace Backend.Server.Config;

public static class Service
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.ConfigureSwagger(configuration);
        services.ConfigureAuth(configuration);
        
        services.AddDbContext<Context>();
        
        // Repositories
        services.AddScoped<Routes.Account.AccountRepository>();
        services.AddScoped<Routes.Product.ProductRepository>();
        
        // Services
        services.AddScoped<Routes.Account.AccountService>();
        services.AddScoped<Routes.Product.ProductService>();

        return services;
    }
}