namespace Backend.Server.Config.Pipelines;

public static class Auth
{
    public static WebApplication ConfigureAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}