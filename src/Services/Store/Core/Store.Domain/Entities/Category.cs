namespace Store.Domain.Entities;

public class Category(string name, string companyId) : BaseEntity(), ICompanyOwned
{
    public string Name { get; private set; } = name;
    public string CompanyId { get; private set; } = companyId;

    public List<Product>? Products { get; }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
