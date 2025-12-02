using Store.Application.Common.Models.Category;

namespace Store.Application.Features.Categories;

public record GetCategoriesQuery() : IRequest<List<CategoryReturnDto>>;

public class GetCategoriesQueryHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetCategoriesQuery, List<CategoryReturnDto>>
{
    public async Task<List<CategoryReturnDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .ProjectToType<CategoryReturnDto>()
            .ToListAsync(cancellationToken);

        return categories;
    }
}