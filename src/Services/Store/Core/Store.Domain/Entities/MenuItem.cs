namespace Store.Domain.Entities;

public class MenuItem(string branchId, string companyId, string productId) : BaseEntity(), ICompanyOwned
{
    public string BranchId { get; private set; } = branchId;
    public string CompanyId { get; private set; } = companyId;
    public string ProductId { get; private set; } = productId;
    public Product? Product { get; private set; }
}