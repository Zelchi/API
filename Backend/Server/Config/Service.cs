using DataShared;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Config;
// Classe responsável por configurar os serviços da aplicação
public static class Service
{
    // Adiciona todos os serviços necessários para a aplicação
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adiciona suporte para controllers
        services.AddControllers();
        // Adiciona explorador de endpoints para documentação da API
        services.AddEndpointsApiExplorer();
        // Adiciona geração de documentação Swagger
        services.AddSwaggerGen();
        // Adiciona o contexto do banco de dados
        services.AddDbContext<Database>(o => o.UseMySQL(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}