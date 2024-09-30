using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Entities;

namespace Organization.Infrastructure.Data.Configurations;

public class CompanyMapping : BaseEntityMapping<Company>
{
    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Id)
            .HasMaxLength(5);

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(120);

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(120);

        builder.HasMany(e => e.Branches)
            .WithOne(b => b.Company)
            .HasForeignKey(b => b.CompanyId);
    }
}
