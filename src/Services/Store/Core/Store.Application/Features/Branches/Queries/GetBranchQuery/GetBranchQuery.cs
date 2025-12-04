using Store.Application.Common.Models.Branch;

namespace Store.Application.Features.Branches;

public record GetBranchQuery(string Id) : IRequest<BranchReturnDto>;

public class GetBranchQueryHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetBranchQuery, BranchReturnDto>
{
    public async Task<BranchReturnDto> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches
            .Where(x => x.Id == request.Id)
            .ProjectToType<BranchReturnDto>()
            .FirstOrDefaultAsync(cancellationToken);
        if(branch is null)
            throw new NotFoundException($"Branch not found with ID: {request.Id}");
        return branch;
    }
}