namespace Backend.Server.Config;

public static class Startup
{
    public static WebApplication CreateApplication()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddServices();

        var app = builder.Build();
        
        return app.ConfigurePipeline();
    }
}