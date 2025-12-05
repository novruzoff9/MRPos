namespace Order.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TableOrder> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
