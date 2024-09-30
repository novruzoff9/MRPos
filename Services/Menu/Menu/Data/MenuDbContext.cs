using Menu.Models;
using Microsoft.EntityFrameworkCore;

namespace Menu.Data;

public class MenuDbContext : DbContext
{
    public MenuDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MenuItem>()
            .HasKey(e => new { e.ProductId, e.BranchId });

        modelBuilder.Entity<MenuItem>()
            .Property(e => e.ProductId)
            .ValueGeneratedNever();
    }
    public DbSet<MenuItem> MenuItems { get; set; }
}
