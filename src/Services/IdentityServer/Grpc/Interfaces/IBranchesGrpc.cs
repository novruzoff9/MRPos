using Branches.Grpc;

namespace IdentityServer.Grpc.Interfaces;

public interface IBranchesGrpc
{
    Task<string> GetBranchNameAsync(string branchId);
    Task<Dictionary<string, string>> GetBranchNamesAsync(IEnumerable<string> branchIds);
}

public class BranchesGrpcImplementation(BranchesGrpc.BranchesGrpcClient client) : IBranchesGrpc
{
    public async Task<string> GetBranchNameAsync(string branchId)
    {
        var request = new BranchNameRequest { BranchId = branchId };
        var response = await client.GetBranchNameByBranchIdAsync(request);
        return response.Name;
    }

    public async Task<Dictionary<string, string>> GetBranchNamesAsync(IEnumerable<string> branchIds)
    {
        var request = new GetBranchNamesBatchRequest();
        request.BranchIds.AddRange(branchIds);
        
        var response = await client.GetBranchNameForBranchIdsAsync(request);
        
        return response.Items.ToDictionary(
            item => item.BranchId,
            item => item.BranchName
        );
    }
}
