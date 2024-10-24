using Shared.ResultTypes;
using Shared.Services;

namespace Organization.Application.Branches.Queries.GetBranchesQuery;

public record GetBranchesofCompany(string id) : IRequest<Response<List<Branch>>>;

public class GetBranchesofCompanyQueryHandler : IRequestHandler<GetBranchesofCompany, Response<List<Branch>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetBranchesofCompanyQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<Branch>>> Handle(GetBranchesofCompany request, CancellationToken cancellationToken)
    {
        var branches = await _context.Branches.Where(x=>x.CompanyId == request.id).ToListAsync(cancellationToken);
        return Response<List<Branch>>.Success(branches, 200);
    }
}

