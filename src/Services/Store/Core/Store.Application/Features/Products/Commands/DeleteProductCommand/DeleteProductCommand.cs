namespace Store.Application.Features.Products;

public record DeleteProductCommand(string Id) : IRequest<bool>;

public class DeleteProductCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        dbContext.Products.Remove(product);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
