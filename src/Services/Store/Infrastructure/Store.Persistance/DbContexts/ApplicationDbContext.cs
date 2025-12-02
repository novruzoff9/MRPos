using Shared.Interfaces;
using Store.Persistance.Extensions;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.ApplyTenantQueryFilter(identityService);
        modelBuilder.ApplyTenantIndexes();
        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
