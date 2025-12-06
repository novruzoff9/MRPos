using Order.Application.Common.Models.Order;
using Order.Application.Common.Models.OrderItem;

namespace Order.Application.Common.Mappings;

internal class OrderMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TableOrder, ReturnOrderDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.TableName, src => src.TableNumber)
            .Map(dest => dest.TotalAmount, src => src.TotalPrice)
            .Map(dest => dest.OrderDate, src => src.OpenedAt.ToString("HH:mm dd MMMM yyyy"))
            .Map(dest => dest.Items, src => src.Items);
        config.NewConfig<OrderItem, ReturnOrderItemDto>()
            .Map(dest => dest.ItemId, src => src.Id)
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Price, src => src.UnitPrice);
    }
}
