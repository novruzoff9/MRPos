using Store.Application.Common.Models.Product;

namespace Store.Application.Features.Products;

public record GetProductQuery(string Id) : IRequest<ProductReturnDto>;

public class GetProductQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetProductQuery, ProductReturnDto>
{
    public async Task<ProductReturnDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .ProjectToType<ProductReturnDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new NotFoundException($"Product not found with ID: {request.Id}");
        return product;
    }
}
