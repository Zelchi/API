namespace Backend.Server.Config.Pipelines;

public static class Cors
{
    public static WebApplication ConfigureCors(this WebApplication app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });

        return app;
    }
}