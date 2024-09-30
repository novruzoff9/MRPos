namespace Catalog.Application.Branches.Queries.GetBranchesQuery;

public record GetBranches : IRequest<List<Branch>>;

public class GetBranchesQueryHandler : IRequestHandler<GetBranches, List<Branch>>
{
    private readonly IApplicationDbContext _context;

    public GetBranchesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Branch>> Handle(GetBranches request, CancellationToken cancellationToken)
    {
        var branches = await _context.Branches.ToListAsync(cancellationToken);
        return branches;
    }
}

