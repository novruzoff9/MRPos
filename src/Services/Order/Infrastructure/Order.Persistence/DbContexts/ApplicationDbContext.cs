using Order.Domain.Entities;
using Order.Persistence.Extensions;
using Shared.Interfaces;
using System.Reflection;

namespace Order.Persistence.DbContexts;

public class ApplicationDbContext(
    IIdentityService identityService,
    DbContextOptions<ApplicationDbContext> options
    ) : DbContext(options), IApplicationDbContext
{

    public DbSet<TableOrder> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyTenantQueryFilter(identityService);
        modelBuilder.ApplyTenantIndexes();
        base.OnModelCreating(modelBuilder);
    }
}
