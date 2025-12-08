using Store.Application.Common.Models.Branch;

namespace Store.Application.Features.Branches;

public record GetBranchesQuery() : IRequest<List<BranchReturnDto>>;

public class GetBranchesQueryHandler(
    IApplicationDbContext dbContext,
    IIdentityGrpcClient identityGrpcClient
    ) : IRequestHandler<GetBranchesQuery, List<BranchReturnDto>>
{
    public async Task<List<BranchReturnDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await dbContext.Branches
            .ProjectToType<BranchReturnDto>()
            .ToListAsync(cancellationToken);

        var result = new List<BranchReturnDto>();
        var ids = branches.Select(b => b.Id).ToList();
        var counts = await identityGrpcClient.GetEmployeeCountsAsync(ids);
        var dict = counts.ToDictionary(x => x.Key, x => x.Value);
        foreach (var branch in branches)
        {
            if (dict.TryGetValue(branch.Id, out int count))
                result.Add(branch with { EmployeeCount = count });
            else
                result.Add(branch);
        }
        return result;
    }
}