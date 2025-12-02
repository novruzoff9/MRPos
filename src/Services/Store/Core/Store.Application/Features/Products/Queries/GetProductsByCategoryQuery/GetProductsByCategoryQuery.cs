using Store.Application.Common.Models.Product;

namespace Store.Application.Features.Products;

public record GetProductsByCategoryQuery(string CategoryId) : IRequest<List<ProductReturnDto>>;

public class GetProductsByCategoryQueryHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetProductsByCategoryQuery, List<ProductReturnDto>>
{
    public async Task<List<ProductReturnDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products.Where(x=> x.CategoryId == request.CategoryId)
            .ProjectToType<ProductReturnDto>()
            .ToListAsync(cancellationToken);
        return products;
    }
}
