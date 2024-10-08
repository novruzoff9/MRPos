using Organization.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Models.Branch;

public class BranchSummaryDto
{
    public string Id { get; set; }
    public string CompanyId { get; set; }
    public bool Is24Hour { get; set; }
    public TimeOnly Opening { get; set; }
    public TimeOnly Closing { get; set; }
    public decimal ServiceFee { get; set; }
}
