using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
}