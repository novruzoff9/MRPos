using Organization.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities;

public class Company : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LogoUrl { get; set; }
    public List<Branch> Branches { get; set; }
}

public class Branch : BaseAuditableEntity
{
    public string CompanyId { get; set; }
    public string GoogleMapsLocation { get; set; }
    public bool Is24Hour { get; set; }
    public TimeOnly Opening { get; set; }
    public TimeOnly Closing { get; set; }
    public decimal ServiceFee { get; set; }
}
