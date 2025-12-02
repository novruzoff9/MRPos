using Store.Application.Common.Models.Category;

namespace Store.Application.Features.Categories;

public record GetCategoriesofCompany(string Id) : IRequest<List<CategoryReturnDto>>;

public class GetCategoriesofCompanyQueryHandler(IApplicationDbContext dbContext
    ) : IRequestHandler<GetCategoriesofCompany, List<CategoryReturnDto>>
{
    public async Task<List<CategoryReturnDto>> Handle(GetCategoriesofCompany request, CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => x.CompanyId == request.Id)
            .ProjectToType<CategoryReturnDto>()
            .ToListAsync(cancellationToken);

        return categories;
    }
}