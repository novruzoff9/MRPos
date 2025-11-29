using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.ResultTypes;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries.GetProductsQuery;

public record GetProducts : IRequest<Response<List<Product>>>;

public class GetProductsQueryHandler : IRequestHandler<GetProducts, Response<List<Product>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetProductsQueryHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<Product>>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await _context.Products.Where(x=>x.CompanyId == _identityService.GetCompanyId).ToListAsync(cancellationToken);
        return Response<List<Product>>.Success(products, 200);
    }
}
