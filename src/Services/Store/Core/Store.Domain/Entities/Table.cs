using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Table(string name, int? capacity, string branchId, decimal? deposit = null) : BaseEntity()
{
    public string Name { get; private set; } = name;
    public int? Capacity { get; private set; } = capacity;
    public decimal? Deposit { get; private set; } = deposit ?? 0;

    public TableStatus TableStatus { get; private set; } = TableStatus.Vacant;

    public string BranchId { get; init; } = branchId;
    public Branch? Branch { get; private set; }

    public void UpdateDeposit(decimal deposit)
    {
        Deposit = deposit;
    }

    public void UpdateStatus(TableStatus status)
    {
        TableStatus = status;
    }
}
