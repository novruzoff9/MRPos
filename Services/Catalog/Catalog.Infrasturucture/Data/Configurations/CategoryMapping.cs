using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrasturucture.Data.Configurations;

public class CategoryMapping : BaseEntityMapping<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.HasMany(e => e.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}