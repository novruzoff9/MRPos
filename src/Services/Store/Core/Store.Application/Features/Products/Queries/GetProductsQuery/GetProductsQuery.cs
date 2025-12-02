using Store.Application.Common.Models.Product;

namespace Store.Application.Features.Products;

public record GetProductsQuery : IRequest<List<ProductReturnDto>>;

public class GetProductsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetProductsQuery, List<ProductReturnDto>>
{
    public async Task<List<ProductReturnDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .ProjectToType<ProductReturnDto>()
            .ToListAsync(cancellationToken);
        return products;
    }
}
