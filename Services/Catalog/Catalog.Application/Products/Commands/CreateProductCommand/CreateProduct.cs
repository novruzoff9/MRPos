using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.Services;

namespace Catalog.Application.Products.Commands.CreateProductCommand;

public record CreateProduct(string Name, string Description, decimal Price, int categoryId) : IRequest<bool>;

public class CreateProductCommandHandler : IRequestHandler<CreateProduct, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public CreateProductCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<bool> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateProduct));

        var Product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.categoryId,
            Created = DateTime.UtcNow,
            CreatedBy = _identityService.GetUserId,
            CompanyId = _identityService.GetCompanyId
        };

        await _context.Products.AddAsync(Product, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}
