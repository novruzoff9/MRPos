using Store.Application.Common.Models.Branch;

namespace Store.Application.Features.Branches;

public record GetBranchesQuery() : IRequest<List<BranchReturnDto>>;

public class GetBranchesQueryHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetBranchesQuery, List<BranchReturnDto>>
{
    public async Task<List<BranchReturnDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await dbContext.Branches
            .ProjectToType<BranchReturnDto>()
            .ToListAsync(cancellationToken);
        return branches;
    }
}