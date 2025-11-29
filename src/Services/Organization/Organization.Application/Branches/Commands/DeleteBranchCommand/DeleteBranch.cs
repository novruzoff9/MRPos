using Shared.Interfaces;

namespace Organization.Application.Branches.Commands.DeleteBranchCommand;

public record DeleteBranch(string Id) : IRequest<bool>;

public class DeleteBranchCommandHandler(IApplicationDbContext context, IIdentityService identityService) : IRequestHandler<DeleteBranch, bool>
{
    public async Task<bool> Handle(DeleteBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var branch = await context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == identityService.GetCompanyId, cancellationToken);
        if (branch == null) { return false; }
        context.Branches.Remove(branch);

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

