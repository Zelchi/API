using Microsoft.EntityFrameworkCore;
using DataShared.Entities;

namespace DataShared;

public class Database(DbContextOptions<Database> options) : DbContext(options)
{
    // DbSets para as entidades
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}