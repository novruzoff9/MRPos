using Shared.Interfaces;
using System.Linq.Expressions;

namespace Order.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyTenantQueryFilter(this ModelBuilder modelBuilder, IIdentityService identityService)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ICompanyOwned).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ICompanyOwned.CompanyId));
                ConstantExpression tenantId;
                try
                {
                    tenantId = Expression.Constant(identityService.GetCompanyId);
                }
                catch (Exception)
                {
                    tenantId = Expression.Constant("-1"); // migration zamanı dummy tenant
                }
                var equal = Expression.Equal(property, tenantId);
                var lambda = Expression.Lambda(equal, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

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