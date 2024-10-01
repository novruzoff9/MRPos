using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Entities;

namespace Organization.Infrastructure.Data.Configurations;

public class TableBuilder : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasKey(e=> new {e.BranchId, e.TableNumber});

        builder.Property(e => e.Deposit)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.BranchId)
            .HasMaxLength(6);

        builder.Property(e => e.TableNumber)
            .HasMaxLength(30);

        builder.HasOne(e=>e.Branch)
            .WithMany(b=>b.Tables)
            .HasForeignKey(e=>e.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}