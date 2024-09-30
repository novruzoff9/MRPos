namespace Catalog.Application.Branches.Commands.DeleteBranchCommand;

public record DeleteBranch(string Id) : IRequest<bool>;

public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranch, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteBranchCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (branch == null) { return false; }
        _context.Branches.Remove(branch);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

