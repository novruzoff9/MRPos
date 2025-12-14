using Shared.Interfaces;
using Store.Domain.Common;
using Store.Persistance.Extensions;
using System.Reflection;

namespace Store.Persistance.DbContexts;

public class ApplicationDbContext(
    IIdentityService identityService,
    DbContextOptions<ApplicationDbContext> options
    ) : DbContext(options), IApplicationDbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    private string _companyId => identityService.GetCompanyId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ICompanyOwned).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(entityType.ClrType);

                method.Invoke(this, [modelBuilder]);
            }
        }
        modelBuilder.ApplyTenantIndexes();
        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
    private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ICompanyOwned
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(e => e.CompanyId == _companyId);
    }
}
