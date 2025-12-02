using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Product(string name, string? description, decimal price, ProductStatus status, string companyId, string categoryId) : BaseEntity(), ICompanyOwned
{
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public decimal Price { get; private set; } = price;
    public ProductStatus Status { get; private set; } = status;
    public string CompanyId { get; private set; } = companyId;

    public string CategoryId { get; private set; } = categoryId;
    public Category? Category { get; }
    public void UpdateStatus(ProductStatus status)
    {
        Status = status;
    }
    public void UpdateDetails(string name, string description, decimal price, string categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }
}
