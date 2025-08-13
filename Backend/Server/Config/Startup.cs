namespace Backend.Server.Config;
// Classe responsável por inicializar e configurar a aplicação
public static class Startup
{
    // Método que cria e configura a aplicação web
    public static WebApplication CreateApplication()
    {
        // Cria o builder da aplicação
        var builder = WebApplication.CreateBuilder();
        // Aplica os novos Services da aplicação
        builder.Services.AddServices(builder.Configuration);
        // Constrói a aplicação
        var app = builder.Build();
        // Configura o pipeline de requisições e retorna a aplicação
        return app.ConfigurePipeline();
    }
}