using Backend.Server.Config.Pipelines;

namespace Backend.Server.Config;

public static class Pipeline
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.ConfigureSwagger(); // Desenvolvimento
        }
        else
        {
            app.ConfigureHttp(); // Produção
            app.ConfigureCors(); // Produção
        }

        app.ConfigureAuth();
        app.MapControllers();

        return app;
    }
}