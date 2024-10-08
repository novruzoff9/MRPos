using Shared.ResultTypes;
using Shared.Services;

namespace Organization.Application.Branches.Queries.GetBranchesQuery;

public record GetBranches : IRequest<Response<List<Branch>>>;

public class GetBranchesQueryHandler : IRequestHandler<GetBranches, Response<List<Branch>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetBranchesQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<Branch>>> Handle(GetBranches request, CancellationToken cancellationToken)
    {
        var companyId = _identityService.GetCompanyId;
        List<Branch> branches = new List<Branch>();
        if(companyId != "MRPos")
        {
            branches = await _context.Branches.Where(x=>x.CompanyId == companyId).ToListAsync(cancellationToken);
        }
        else
        {
            branches = await _context.Branches.ToListAsync(cancellationToken);
        }
        return Response<List<Branch>>.Success(branches, 200);
    }
}

