using Shared.ResultTypes;
using Store.Domain.Enums;

namespace Store.Application.Features.Products;

public record CreateProductCommand(string Name, string? Description, decimal Price, ProductStatus Status, string CategoryId) : IRequest<bool>;

public class CreateProductCommandHandler(
    IApplicationDbContext dbContext, 
    IIdentityService identityService
    ) : IRequestHandler<CreateProductCommand, bool>
{
    public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Status,
            identityService.GetCompanyId,
            request.CategoryId
        );

        await dbContext.Products.AddAsync(product, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
