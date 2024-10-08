using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.ResultTypes;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Categories.Queries.GetCategoryQuery;

public record GetCategory(int Id) : IRequest<Response<Category>>;

public class GetCategoryQueryHandler : IRequestHandler<GetCategory, Response<Category>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetCategoryQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<Category>> Handle(GetCategory request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);
        return Response<Category>.Success(category, 200);
    }
}
