using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Queries.GetProductQuery;

public record GetProduct(int Id) : IRequest<Product>;

public class GetProductQueryHandler : IRequestHandler<GetProduct, Product>
{
    private readonly IApplicationDbContext _context;

    public GetProductQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Handle(GetProduct request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return product;
    }
}
