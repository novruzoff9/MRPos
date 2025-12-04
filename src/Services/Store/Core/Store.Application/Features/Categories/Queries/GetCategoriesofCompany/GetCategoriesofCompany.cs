using Store.Application.Common.Models.Category;

namespace Store.Application.Features.Categories;

public record GetCategoriesofCompany(string CompanyId) : IRequest<List<CategoryReturnDto>>;

public class GetCategoriesofCompanyQueryHandler(IApplicationDbContext dbContext
    ) : IRequestHandler<GetCategoriesofCompany, List<CategoryReturnDto>>
{
    public async Task<List<CategoryReturnDto>> Handle(GetCategoriesofCompany request, CancellationToken cancellationToken)
    {
        var companyExists = await dbContext.Companies
            .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

        if (!companyExists)
            throw new NotFoundException($"Company not found with ID: {request.CompanyId}");

        var categories = await dbContext.Categories
            .Where(x => x.CompanyId == request.CompanyId)
            .ProjectToType<CategoryReturnDto>()
            .ToListAsync(cancellationToken);

        return categories;
    }
}