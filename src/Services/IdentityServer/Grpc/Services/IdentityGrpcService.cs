using Grpc.Core;
using Identity.Grpc;
using IdentityServer.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Grpc.Services;

public class IdentityGrpcService(IdentityDbContext dbContext) : IdentityGrpc.IdentityGrpcBase
{
    public override async Task<EmployeeCountResponse> GetEmployeeCountByBranch(EmployeeCountRequest request, ServerCallContext context)
    {
        string branchId = request.BranchId;
        var count = await dbContext.Users.CountAsync(e => e.BranchId == branchId);
        var response = new EmployeeCountResponse
        {
            Count = count
        };
        return response;
    }

    public override async Task<GetEmployeeCountBatchResponse> GetEmployeeCountForBranches(GetEmployeeCountBatchRequest request, ServerCallContext context)
    {
        var response = new GetEmployeeCountBatchResponse();
        var branchIds = request.BranchIds;
        var counts = await dbContext.Users
            .Where(e => branchIds.Contains(e.BranchId))
            .GroupBy(e => e.BranchId)
            .Select(g => new { BranchId = g.Key, Count = g.Count() })
            .ToListAsync();
        foreach (var item in counts)
        {
            var branchCount = new EmployeeCountItem
            {
                BranchId = item.BranchId,
                Count = item.Count
            };
            response.Items.Add(branchCount);
        }
        return response;
    }
}
