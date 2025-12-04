using Store.Application.Common.Models.Category;

namespace Store.Application.Features.Categories;

public record GetCategoryQuery(string Id) : IRequest<CategoryReturnDto>;

public class GetCategoryQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCategoryQuery, CategoryReturnDto>
{
    public async Task<CategoryReturnDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .ProjectToType<CategoryReturnDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category is null)
            throw new NotFoundException($"Category not found with ID: {request.Id}");
        return category;
    }
}
