using Organization.Domain.Common;

namespace Organization.Domain.Entities;

public class Table
{
    public string BranchId { get; set; }
    public Branch Branch { get; set; }
    public string TableNumber { get; set; }
    public decimal Deposit { get; set; }
}