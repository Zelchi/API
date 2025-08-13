using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Config;

public class Database(DbContextOptions<Database> options) : DbContext(options)
{
    public DbSet<Routes.Contact.ContactEntity> Contacts { get; set; }
    public DbSet<Routes.Product.ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}