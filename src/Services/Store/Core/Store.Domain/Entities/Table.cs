namespace Store.Domain.Entities;

public class Table(string name, int? capacity, string branchId, decimal? deposit = null) : BaseEntity()
{
    public string Name { get; private set; } = name;
    public int? Capacity { get; private set; } = capacity;
    public decimal? Deposit { get; private set; } = deposit ?? 0;

    public string BranchId { get; private set; } = branchId;
    public Branch? Branch { get; }

    public void UpdateDeposit(decimal deposit)
    {
        Deposit = deposit;
    }
}
