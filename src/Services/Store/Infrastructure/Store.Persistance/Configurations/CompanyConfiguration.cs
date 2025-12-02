using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Persistance.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(200);

        builder.HasMany(c => c.Branches)
            .WithOne(c => c.Company)
            .HasForeignKey(c => c.CompanyId);
    }
}
