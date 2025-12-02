namespace Store.Application.Features.Categories;

public record CreateCategoryCommand(string Name) : IRequest<bool>;

public class CreateCategoryCommandHandler(
    IApplicationDbContext dbContext, 
    IIdentityService identityService
    ) : IRequestHandler<CreateCategoryCommand, bool>
{
    public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        string companyId = identityService.GetCompanyId;
        var category = new Category(request.Name, companyId);
        await dbContext.Categories.AddAsync(category, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}