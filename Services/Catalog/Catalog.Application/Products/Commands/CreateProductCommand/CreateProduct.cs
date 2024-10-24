using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Shared.ResultTypes;
using Shared.Services;

namespace Catalog.Application.Products.Commands.CreateProductCommand;

public record CreateProduct(string Name, string Description, decimal Price, int categoryId) : IRequest<Response<NoContent>>;

public class CreateProductCommandHandler : IRequestHandler<CreateProduct, Response<NoContent>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public CreateProductCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Response<NoContent>> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateProduct));

        var Product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.categoryId,
            Created = DateTime.UtcNow,
            //CreatedBy = _identityService.GetUserId,
            CompanyId = _identityService.GetCompanyId
        };

        await _context.Products.AddAsync(Product, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Response<NoContent>.Success(200);
    }
}
