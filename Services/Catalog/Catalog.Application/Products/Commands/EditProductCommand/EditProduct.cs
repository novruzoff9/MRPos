using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Products.Commands.EditProductCommand;

public record EditProduct(int Id, string Name, string Description, decimal Price) : IRequest<bool>;

public class EditProductCommandHanlder : IRequestHandler<EditProduct, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public EditProductCommandHanlder(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<bool> Handle(EditProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.Name, nameof(request));
        Guard.Against.NotFound(request.Description, nameof(request));
        Guard.Against.NegativeOrZero(request.Price, nameof(request));

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product == null) { return false; }
        if (product.CompanyId != _identityService.GetCompanyId) { return false; }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Modified = DateTime.UtcNow;
        product.LastModifiedBy = _identityService.GetUserId;

        _context.Products.Update(product);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
