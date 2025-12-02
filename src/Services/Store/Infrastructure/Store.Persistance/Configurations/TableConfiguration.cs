using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Persistance.Configurations;

internal class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Deposit).HasColumnType("decimal(18,2)");
        builder.HasOne(t => t.Branch)
               .WithMany(b => b.Tables)
               .HasForeignKey(t => t.BranchId);
    }
}
