using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Config;

public static class Service
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<Database>((options) =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}