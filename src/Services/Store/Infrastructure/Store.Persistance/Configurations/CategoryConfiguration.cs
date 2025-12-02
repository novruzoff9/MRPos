using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Persistance.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c=>c.CompanyId).IsRequired();
        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId);
    }
}
