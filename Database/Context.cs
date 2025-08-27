using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database;

public class Context(DbContextOptions<Context> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string url = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string 'DefaultConnection' not found.");
        options.UseMySQL(url);
    }
}