using Order.Application.Common.Models.OrderItem;

namespace Order.Application.Common.Models.Order;

public record ReturnOrderDto(string Id, string TableName, decimal TotalAmount, string OrderDate, 
    List<ReturnOrderItemDto> Items);