namespace Catalog.Application.Branches.Queries.GetBranchQuery;

public record GetBranch(string Id) : IRequest<Branch>;

public class GetBranchQueryHandler : IRequestHandler<GetBranch, Branch>
{
    private readonly IApplicationDbContext _context;

    public GetBranchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Branch> Handle(GetBranch request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return branch;
    }
}

