namespace Menu.Models;

public class MenuItem
{
    public int ProductId { get; set; }
    public string BranchId { get; set; }
    public string ProductName { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
