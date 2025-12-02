using Store.Application.Common.Models.Branch;

namespace Store.Application.Features.Branches;

public record GetBranchQuery(string Id) : IRequest<BranchReturnDto>;

public class GetBranchQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetBranchQuery, BranchReturnDto>
{
    public async Task<BranchReturnDto> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches
            .ProjectToType<BranchReturnDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        //TODO: Exception
        return branch;
    }
}