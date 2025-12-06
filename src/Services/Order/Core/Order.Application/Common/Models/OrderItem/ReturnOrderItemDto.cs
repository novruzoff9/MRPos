namespace Order.Application.Common.Models.OrderItem;

public record ReturnOrderItemDto(string ItemId, string ProductName, decimal Price, int Quantity, decimal TotalPrice);