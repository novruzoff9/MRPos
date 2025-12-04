namespace Store.Application.Features.Branches;

public record DeleteBranchCommand(string Id) : IRequest<bool>;

public class DeleteBranchCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteBranchCommand, bool>
{
    public async Task<bool> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =  await dbContext.Branches
            .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
        if(branch is null)
            throw new NotFoundException($"Branch not found with ID: {request.Id}");
        dbContext.Branches.Remove(branch);
        return true;
    }
}
