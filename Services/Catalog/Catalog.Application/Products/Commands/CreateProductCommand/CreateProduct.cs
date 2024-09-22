using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Commands.CreateProductCommand;

public record CreateProduct(string Name, string Description, decimal Price) : IRequest<bool>;

public class CreateProductCommandHandler : IRequestHandler<CreateProduct, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateProduct));

        var Product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Created = DateTime.UtcNow
        };

        await _context.Products.AddAsync(Product, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}
