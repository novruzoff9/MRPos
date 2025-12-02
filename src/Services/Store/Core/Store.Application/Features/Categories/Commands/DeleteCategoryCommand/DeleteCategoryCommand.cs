namespace Store.Application.Features.Categories;

public record DeleteCategoryCommand(string Id) : IRequest<bool>;

public class DeleteCategoryCommandHandler(
    IApplicationDbContext dbContext
    ) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        dbContext.Categories.Remove(category);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
