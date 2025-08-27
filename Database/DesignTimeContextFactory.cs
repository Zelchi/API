using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var basePath = Directory.GetCurrentDirectory();

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();

        var configuration = configBuilder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<Context>()
            .UseMySQL(connectionString);

        return new Context(optionsBuilder.Options);
    }
}
