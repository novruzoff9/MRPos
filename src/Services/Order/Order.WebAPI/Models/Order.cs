namespace Order.WebAPI.Models;

public class BaseEntity
{
    public int Id { get; set; }
}

public class TableOrder : BaseEntity
{
    public DateTime Opened { get; set; }
    public DateTime Closed { get; set; }
    public List<OrderItem> Items { get; set; }
    public string WaiterId { get; set; }
    public string BranchId { get; set; }
    public string TableNumber { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Deposit { get; set; }
    public decimal TotalPrice => Items?.Sum(o => o.Quantity * o.Price) ?? 0m;
    public bool Completed { get; set; }
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public TableOrder Order { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
