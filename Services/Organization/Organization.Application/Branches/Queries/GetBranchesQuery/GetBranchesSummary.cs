using AutoMapper;
using Organization.Application.Common.Models.Branch;
using Shared.ResultTypes;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Branches.Queries.GetBranchesQuery;

public record GetBranchesSummary : IRequest<Response<List<BranchSummaryDto>>>;

public class GetBranchesSummaryQueryHandler : IRequestHandler<GetBranchesSummary, Response<List<BranchSummaryDto>>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetBranchesSummaryQueryHandler(IMapper mapper, IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _mapper = mapper;
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<BranchSummaryDto>>> Handle(GetBranchesSummary request, CancellationToken cancellationToken)
    {
        var companyId = _identityService.GetCompanyId;
        List<BranchSummaryDto> branches = new List<BranchSummaryDto>();
        if (companyId != "MRPos")
        {
            var detbranches = await _context.Branches.Where(x => x.CompanyId == companyId).ToListAsync(cancellationToken);
            branches = _mapper.Map<List<BranchSummaryDto>>(detbranches);
        }
        else
        {
            var detbranches = await _context.Branches.ToListAsync(cancellationToken);
            branches = _mapper.Map<List<BranchSummaryDto>>(detbranches);
        }

        return Response<List<BranchSummaryDto>>.Success(branches, 200);
    }
}
