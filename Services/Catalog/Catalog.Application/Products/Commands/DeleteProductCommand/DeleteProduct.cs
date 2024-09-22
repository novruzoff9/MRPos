using Catalog.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Commands.DeleteProductCommand;

public record DeleteProduct(int Id) : IRequest<bool>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProduct, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product == null) { return false; }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
