using Order.Application.Common.Models.Order;
using Shared.Exceptions;

namespace Order.Application.Features.TableOrders;

public record GetTableOrderQuery(string Id) : IRequest<ReturnOrderDto>;

public class GetTableOrderQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTableOrderQuery, ReturnOrderDto>
{
    public async Task<ReturnOrderDto> Handle(GetTableOrderQuery request, CancellationToken cancellationToken)
    {
        var tableorder = await dbContext.Orders
            .Include(x => x.Items)
            .Where(x => x.Id == request.Id)
            .ProjectToType<ReturnOrderDto>()
            .FirstOrDefaultAsync(cancellationToken);
        if (tableorder == null)
            throw new NotFoundException($"Order with Id {request.Id} not found.");
        return tableorder;
    }
}