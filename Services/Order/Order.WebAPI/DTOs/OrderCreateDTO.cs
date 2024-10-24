using Order.WebAPI.Models;

namespace Order.WebAPI.DTOs;

public class OrderDTO
{
    public string BranchId { get; set; }
    public string TableNumber { get; set; }
}

public class AddOrderItemDto
{
    public int OrderId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class OrderItemDto
{
    public decimal Price { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}