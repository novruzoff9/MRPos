using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries.GetProductsQuery;

public record GetProducts : IRequest<List<Product>>;

public class GetProductsQueryHandler : IRequestHandler<GetProducts, List<Product>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetProductsQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<List<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await _context.Products.Where(x=>x.CompanyId == _identityService.GetCompanyId).ToListAsync(cancellationToken);
        return products;
    }
}
