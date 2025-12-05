namespace Order.Domain.Entities;

public class OrderItem(string orderId, string productId, string productName, int quantity, decimal unitPrice) : BaseEntity()
{
    public string OrderId { get; init; } = orderId;
    public TableOrder? Order { get; private set; }
    public string ProductId { get; init; } = productId;
    public string ProductName { get; private set; } = productName;
    public int Quantity { get; private set; } = quantity;
    public decimal UnitPrice { get; init; } = unitPrice;
    public decimal TotalPrice => UnitPrice * Quantity;

    public void IncreaseQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        Quantity += additionalQuantity;
    }
}