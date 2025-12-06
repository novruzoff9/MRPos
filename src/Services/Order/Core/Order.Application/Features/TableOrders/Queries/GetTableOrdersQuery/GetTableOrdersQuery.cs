using Order.Application.Common.Models.Order;

namespace Order.Application.Features.TableOrders;

public record GetTableOrdersQuery : IRequest<ICollection<ReturnOrderDto>>;

public class GetTableOrdersQueryHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<GetTableOrdersQuery, ICollection<ReturnOrderDto>>
{
    public async Task<ICollection<ReturnOrderDto>> Handle(GetTableOrdersQuery request, CancellationToken cancellationToken)
    {
        var tableorders = await dbContext.Orders
            .Include(x=>x.Items)
            .ProjectToType<ReturnOrderDto>()
            .ToListAsync(cancellationToken);
        return tableorders;
    }
}

