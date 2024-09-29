using Organization.Domain.Common;

namespace Organization.Domain.Entities;

public class Branch : BaseAuditableEntity
{
    public string CompanyId { get; set; } = null!;
    public Company Company { get; set; }
    public string GoogleMapsLocation { get; set; }
    public bool Is24Hour { get; set; }
    public TimeOnly Opening { get; set; }
    public TimeOnly Closing { get; set; }
    public decimal ServiceFee { get; set; }
}
