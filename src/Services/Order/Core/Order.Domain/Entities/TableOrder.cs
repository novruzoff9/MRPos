namespace Order.Domain.Entities;

public class TableOrder : BaseAuditableEntity, ICompanyOwned
{
    public TableOrder(string branchId, string tableNumber, string companyId, decimal deposit,
    decimal servicePercentage, string? waiterId, ICollection<OrderItem> items)
    {
        BranchId = branchId;
        TableNumber = tableNumber;
        CompanyId = companyId;
        Deposit = deposit;
        ServicePercentage = servicePercentage;
        WaiterId = waiterId;
        Items = items ?? [];
    }

    private TableOrder() { } 
    public DateTime OpenedAt { get; init; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; private set; }
    public string? WaiterId { get; private set; }
    public string BranchId { get; init; }
    public string TableNumber { get; init; }
    public decimal Deposit { get; init; }
    public decimal ServicePercentage { get; init; }
    public bool Completed { get; private set; } = false;
    public decimal TotalPrice
    {
        get
        {
            decimal totalPriceWithoutService = Items.Sum(item => item.TotalPrice);
            decimal totalPrice = totalPriceWithoutService + (totalPriceWithoutService * ServicePercentage / 100);
            return totalPrice > Deposit ? totalPrice : Deposit;
        }
    }
    public ICollection<OrderItem> Items { get; private set; }

    public string CompanyId { get; init; }

    public void CloseOrder()
    {
        if (Completed)
            throw new InvalidOperationException("Order is already completed.");
        ClosedAt = DateTime.UtcNow;
        Completed = true;
    }

    public void ChangeWaiter(string? newWaiterId)
    {
        if(Completed)
            throw new InvalidOperationException("Cannot change waiter for a completed order.");
        WaiterId = newWaiterId;
    }

    public void AddItem(OrderItem newItem)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductId == newItem.ProductId);
        if (existingItem != null)
            existingItem.IncreaseQuantity(newItem.Quantity);
        else
            Items.Add(newItem);
    }
    public void RemoveItem(string productId)
    {
        var itemToRemove = Items.FirstOrDefault(item => item.ProductId == productId);
        if (itemToRemove != null)
            Items.Remove(itemToRemove);
        else
            throw new InvalidOperationException("Item not found in the order.");
    }

    public void AddItems(IEnumerable<OrderItem> newItems)
    {
        foreach (var newItem in newItems)
        {
            AddItem(newItem);
        }
    }
}
