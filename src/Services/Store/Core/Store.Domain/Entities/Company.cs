namespace Store.Domain.Entities;

public class Company(string name, string phoneNumber, string? description, string? logoUrl) : BaseEntity(), ISoftDeletable
{
    public string Name { get; private set; } = name;
    public string PhoneNumber { get; private set; } = phoneNumber;
    public string? Description { get; private set; } = description ?? string.Empty;
    public string? LogoUrl { get; private set; } = logoUrl ?? string.Empty;

    public ICollection<Branch>? Branches { get; }

    public DateTime? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
