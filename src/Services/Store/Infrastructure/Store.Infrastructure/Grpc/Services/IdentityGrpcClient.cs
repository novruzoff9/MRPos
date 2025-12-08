using Identity.Grpc;
using Store.Application.Common.Interfaces;

namespace Store.Infrastructure.Grpc.Services;

public class IdentityGrpcClient(
    IdentityGrpc.IdentityGrpcClient identityClient
    ) : IIdentityGrpcClient
{
    public async Task<int> GetEmployeeCountByBranchAsync(string branchId)
    {
        var request = new EmployeeCountRequest
        {
            BranchId = branchId
        };
        var response = await identityClient.GetEmployeeCountByBranchAsync(request);
        return response.Count;
    }

    public async Task<Dictionary<string, int>> GetEmployeeCountsAsync(ICollection<string> branchIds)
    {
        var request = new GetEmployeeCountBatchRequest();
        request.BranchIds.AddRange(branchIds);
        var response = await identityClient.GetEmployeeCountForBranchesAsync(request);
        return response.Items.ToDictionary(kv => kv.BranchId, kv => kv.Count);
    }
}
