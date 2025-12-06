namespace Order.Application.Common.Models.OrderItem;

public record CreateOrderItemDto(string ProductId, int Quantity, decimal UnitPrice, string Name);