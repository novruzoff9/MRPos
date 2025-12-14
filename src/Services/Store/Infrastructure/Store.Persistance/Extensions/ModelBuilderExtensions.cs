using Shared.Interfaces;
using Store.Domain.Common;
using Store.Persistance.DbContexts;
using System.Linq.Expressions;

namespace Store.Persistance.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTenantIndexes(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ICompanyOwned).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasIndex(nameof(ICompanyOwned.CompanyId));
            }
        }
    }

}
