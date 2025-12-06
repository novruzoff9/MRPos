using Order.Application.Common.Models.OrderItem;

namespace Order.Application.Common.Mappings;

internal class OrderItemMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOrderItemDto, OrderItem>()
            .MapWith(dto => new OrderItem(
                orderId: string.Empty,
                productId: dto.ProductId,
                productName: dto.Name,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice));
    }
}
