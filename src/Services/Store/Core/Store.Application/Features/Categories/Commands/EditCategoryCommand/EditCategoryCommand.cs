namespace Store.Application.Features.Categories;

public record EditCategoryCommand(string Id, string Name) : IRequest<bool>;

public class EditCategoryCommandHandler(
    IApplicationDbContext dbContext, 
    IIdentityService identityService
    ) : IRequestHandler<EditCategoryCommand, bool>
{
    public async Task<bool> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null)
            throw new NotFoundException($"Category not found with ID: {request.Id}");
        category.UpdateName(request.Name);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
