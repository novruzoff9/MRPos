using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
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

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await _context.Products.ToListAsync(cancellationToken);
        return products;
    }
}
