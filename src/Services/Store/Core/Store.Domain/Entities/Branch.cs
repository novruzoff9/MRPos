namespace Store.Domain.Entities;

public class Branch : BaseEntity, ICompanyOwned
{
    private Branch() { }

    public Branch(
        string name, string phoneNumber, string? description,
        bool is24Hour, TimeOnly? opening, TimeOnly? closing,
        decimal serviceFee, Address address, string companyId)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Description = description;
        Is24Hour = is24Hour;
        Opening = opening;
        Closing = closing;
        ServiceFee = serviceFee;
        Address = address;
        CompanyId = companyId;
    }

    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Description { get; private set; }
    public bool Is24Hour { get; private set; }
    public TimeOnly? Opening { get; private set; }
    public TimeOnly? Closing { get; private set; }
    public decimal ServiceFee { get; private set; }
    public Address Address { get; private set; }

    public string CompanyId { get; private set; }
    public Company? Company { get; }

    public ICollection<Table> Tables { get; set; }

    public void UpdateOperatingHours(bool is24Hour, TimeOnly? opening, TimeOnly? closing)
    {
        Is24Hour = is24Hour;
        Opening = opening;
        Closing = closing;
    }
}

