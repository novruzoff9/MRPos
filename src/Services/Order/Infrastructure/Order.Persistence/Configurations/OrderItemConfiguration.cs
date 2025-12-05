using Order.Domain.Entities;

namespace Order.Persistence.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.ProductId)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");
        builder.Property(oi => oi.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.Ignore(o => o.TotalPrice);

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}