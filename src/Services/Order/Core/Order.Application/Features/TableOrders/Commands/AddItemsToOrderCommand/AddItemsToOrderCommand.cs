using Order.Application.Common.Models.OrderItem;
using Shared.Exceptions;

namespace Order.Application.Features.TableOrders;

public record AddItemsToOrderCommand(string OrderId, List<CreateOrderItemDto> Items) : IRequest<bool>;

public class AddItemsToOrderCommandHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<AddItemsToOrderCommand, bool>
{
    public async Task<bool> Handle(AddItemsToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            throw new NotFoundException($"Order with Id {request.OrderId} not found.");

        var existing = order.Items.ToDictionary(i => i.ProductId);

        foreach (var dto in request.Items)
        {
            if (existing.TryGetValue(dto.ProductId, out var item))
                item.IncreaseQuantity(dto.Quantity);
            else
            {
                var newItem = new OrderItem(order.Id, dto.ProductId, dto.Name, dto.Quantity, dto.UnitPrice);

                order.Items.Add(newItem);
                existing.Add(dto.ProductId, newItem);
            }
        }

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

