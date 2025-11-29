namespace Catalog.Application.Categories.Queries.GetCategoriesQuery;

public record GetCategoriesofCompany(string id) : IRequest<Response<List<Category>>>;

public class GetCategoriesofCompanyQueryHandler(IApplicationDbContext context, IIdentityService identityService) : IRequestHandler<GetCategoriesofCompany, Response<List<Category>>>
{
    public async Task<Response<List<Category>>> Handle(GetCategoriesofCompany request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories.Where(x=>x.CompanyId == request.id).ToListAsync(cancellationToken);
        return Response<List<Category>>.Success(categories, 200);
    }
}