namespace Backend.Server.Config;
// Classe responsável por configurar o pipeline de requisições HTTP
public static class Pipeline
{
    // Configura a ordem dos middlewares no pipeline de requisições
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Habilita Swagger apenas em desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Força redirecionamento HTTPS apenas em produção
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        // Habilita middleware de autorização para verificar permissões
        app.UseAuthorization();
        // Mapeia os controllers para as rotas da API
        app.MapControllers();

        return app;
    }
}