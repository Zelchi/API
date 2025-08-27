namespace Backend.Server.Config.Pipelines;

public static class Http
{
    public static WebApplication ConfigureHttp(this WebApplication app)
    {
        app.UseHttpsRedirection();

        return app;
    }
}