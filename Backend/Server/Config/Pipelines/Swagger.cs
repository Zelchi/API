namespace Backend.Server.Config.Pipelines;

public static class Swagger
{
    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}