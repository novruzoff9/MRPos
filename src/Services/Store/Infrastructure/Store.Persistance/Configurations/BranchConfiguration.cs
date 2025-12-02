using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Persistance.Configurations;

internal class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.ServiceFee)
            .HasPrecision(10, 2);

        builder.OwnsOne(b => b.Address, a =>
        {
            a.Property(p => p.Street).HasMaxLength(200);
            a.Property(p => p.City).HasMaxLength(100);
            a.Property(p => p.State).HasMaxLength(50);
            a.Property(p => p.ZipCode).HasMaxLength(20);
            a.Property(p => p.Country).HasMaxLength(100);
            a.Property(p => p.GoogleMapsLocation).HasMaxLength(200);
        });

        // Relationships
        //builder.HasMany(b => b.Tables)
        //       .WithOne(t => t.Branch)
        //       .HasForeignKey(t => t.BranchId)
        //       .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Company)
               .WithMany(c => c.Branches)
               .HasForeignKey(b => b.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
