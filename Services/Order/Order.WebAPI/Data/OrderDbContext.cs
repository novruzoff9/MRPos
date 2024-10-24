using Order.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Menu.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TableOrder>()
            .HasKey(e => new { e.Id });

        modelBuilder.Entity<TableOrder>()
            .HasMany(e => e.Items)
            .WithOne(e => e.Order)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(e => e.OrderId);

        modelBuilder.Entity<TableOrder>()
            .Property(e => e.ServiceFee)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<TableOrder>()
            .Property(e => e.Deposit)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .HasKey(e => new { e.Id });

        modelBuilder.Entity<OrderItem>()
            .Property(e => e.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Order)
            .WithMany(e => e.Items)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(e => e.OrderId);

    }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<TableOrder> Orders { get; set; }
}
