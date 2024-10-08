using Organization.Domain.Entities;
using Shared.ResultTypes;
using Shared.Services;

namespace Organization.Application.Branches.Queries.GetBranchQuery;

public record GetBranch(string Id) : IRequest<Response<Branch>>;

public class GetBranchQueryHandler : IRequestHandler<GetBranch, Response<Branch>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetBranchQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<Branch>> Handle(GetBranch request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);
        return Response<Branch>.Success(branch, 200);
    }
}

