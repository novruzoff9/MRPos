using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.ResultTypes;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries.GetProductsByCategoryQuery;

public record GetProductsByCategory(int categoryId) : IRequest<Response<List<Product>>>;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategory, Response<List<Product>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetProductsByCategoryQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<List<Product>>> Handle(GetProductsByCategory request, CancellationToken cancellationToken)
    {
        var products = await _context.Products.Where(x=> x.CategoryId == request.categoryId).ToListAsync(cancellationToken);
        return Response<List<Product>>.Success(products, 200);
    }
}
