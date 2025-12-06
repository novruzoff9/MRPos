namespace Store.Domain.Entities;

public class MenuItem(string branchId, string companyId, string productId) : BaseEntity(), ICompanyOwned
{
    public string BranchId { get; init; } = branchId;
    public string CompanyId { get; init; } = companyId;
    public string ProductId { get; init; } = productId;
    public Product? Product { get; private set; }
}