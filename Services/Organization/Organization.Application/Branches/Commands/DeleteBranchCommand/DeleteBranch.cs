using Shared.Services;

namespace Organization.Application.Branches.Commands.DeleteBranchCommand;

public record DeleteBranch(string Id) : IRequest<bool>;

public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranch, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public DeleteBranchCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<bool> Handle(DeleteBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);
        if (branch == null) { return false; }
        _context.Branches.Remove(branch);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

