using Order.Domain.Entities;

namespace Order.Persistence.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<TableOrder>
{
    public void Configure(EntityTypeBuilder<TableOrder> builder)
    {
        builder.Property(o => o.BranchId)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(o => o.TableNumber)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(o => o.WaiterId)
            .HasMaxLength(30);
        builder.Property(o => o.ServicePercentage)
            .HasColumnType("decimal(18,2)");
        builder.Property(o => o.Deposit)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}