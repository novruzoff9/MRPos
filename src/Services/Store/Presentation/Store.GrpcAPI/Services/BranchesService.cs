using Branches.Grpc;

namespace Store.GrpcAPI.Services;

public class BranchesService(
    IApplicationDbContext dbContext,
    ILogger<BranchesService> logger
    ) : BranchesGrpc.BranchesGrpcBase
{
    public override async Task<BranchNameResponse> GetBranchNameByBranchId(
        BranchNameRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("gRPC: GetBranchNameByBranchId called with Id={Id}", request.BranchId);

        if (string.IsNullOrWhiteSpace(request.BranchId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "BranchId is required."));

        var branchName = await dbContext.Branches
            .IgnoreQueryFilters()
            .Where(x => x.Id == request.BranchId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

        if (branchName is null)
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Branch with Id {request.BranchId} not found.")
            );

        return new BranchNameResponse { Name = branchName };
    }

    public override async Task<GetBranchNamesBatchResponse> GetBranchNameForBranchIds(
        GetBranchNamesBatchRequest request,
        ServerCallContext context)
    {
        logger.LogInformation(
            "gRPC: GetBranchNameForBranchIds called with {Count} IDs",
            request.BranchIds.Count
        );

        var response = new GetBranchNamesBatchResponse();

        if (request.BranchIds.Count == 0)
            return response;

        var branchIds = request.BranchIds.Distinct().ToList();

        var branches = await dbContext.Branches
            .IgnoreQueryFilters()
            .Where(x => branchIds.Contains(x.Id))
            .Select(x => new { x.Id, x.Name })
            .ToDictionaryAsync(x => x.Id, x => x.Name);

        foreach (var id in branchIds)
        {
            response.Items.Add(new BranchNameItem
            {
                BranchId = id,
                BranchName = branches.TryGetValue(id, out var name) ? name : string.Empty,
            });
        }

        return response;
    }
}
