using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Entities;

namespace Organization.Infrastructure.Data.Configurations;

public class BranchMapping : BaseEntityMapping<Branch>
{
    public override void Configure(EntityTypeBuilder<Branch> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Id)
            .HasMaxLength(6);

        builder.Property(e => e.ServiceFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(120);

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(120);

        builder.OwnsOne(e => e.Address)
            .WithOwner();

        builder.HasOne(e => e.Company)
            .WithMany(b => b.Branches)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Tables)
            .WithOne(t => t.Branch)
            .HasForeignKey(e => e.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
