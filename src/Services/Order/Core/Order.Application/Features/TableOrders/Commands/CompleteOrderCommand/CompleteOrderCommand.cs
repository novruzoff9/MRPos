using Shared.Exceptions;

namespace Order.Application.Features.TableOrders;

public record CompleteOrderCommand(string OrderId) : IRequest<bool>;

public class CompleteOrderCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CompleteOrderCommand, bool>
{
    public async Task<bool> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);
        if (order == null)
            throw new NotFoundException($"Order with Id {request.OrderId} not found.");

        order.CloseOrder();
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}