namespace Store.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Table> Tables { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
