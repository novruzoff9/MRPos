using Organization.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities;

public class Company : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LogoUrl { get; set; }
    public List<Branch>? Branches { get; set; }
}
