using Catalog.Application.Categories.Queries.GetCategoryQuery;
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

namespace Catalog.Application.Categories.Queries.GetCategoriesQuery;

public record GetCategoriesofCompany(string id) : IRequest<Response<List<Category>>>;

public class GetCategoriesofCompanyQueryHandler : IRequestHandler<GetCategoriesofCompany, Response<List<Category>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetCategoriesofCompanyQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<Category>>> Handle(GetCategoriesofCompany request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Where(x=>x.CompanyId == request.id).ToListAsync(cancellationToken);
        return Response<List<Category>>.Success(categories, 200);
    }
}