namespace Store.Application.Common.Interfaces;

public interface IIdentityGrpcClient
{
    Task<int> GetEmployeeCountByBranchAsync(string branchId);
    Task<Dictionary<string, int>> GetEmployeeCountsAsync(ICollection<string> branchIds);
}