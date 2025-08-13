using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Config;

public class Database(DbContextOptions<Database> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<Routes.Contact.ContactEntity> Contacts { get; set; }
    public DbSet<Routes.Product.ProductEntity> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured) return;
        options.UseMySQL(configuration.GetConnectionString("DefaultConnection"));
    }
}