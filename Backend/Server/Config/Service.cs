namespace Backend.Server.Config;

public static class Service
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<Database>();
        services.AddScoped<Routes.Contact.ContactService>();
        services.AddScoped<Routes.Product.ProductService>();

        return services;
    }
}