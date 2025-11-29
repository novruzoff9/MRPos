using Organization.Domain.ValueObjects;
using Shared.Interfaces;
using System.Runtime.InteropServices;

namespace Organization.Application.Branches.Commands.EditBranchCommand;

public record EditBranch(string Id, bool Is24Hour, decimal ServiceFee, TimeOnly Opening, TimeOnly Closing, Address Address) : IRequest<bool>;

public class EditBranchCommandHandler : IRequestHandler<EditBranch, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public EditBranchCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<bool> Handle(EditBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.Negative(request.ServiceFee, nameof(request));

        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == _identityService.GetCompanyId, cancellationToken);

        if (branch == null) { return false; }

        branch.ServiceFee = request.ServiceFee;
        branch.Is24Hour = request.Is24Hour;
        branch.Opening = request.Opening;
        branch.Closing = request.Closing;
        branch.LastModified = DateTime.UtcNow;
        branch.Address = request.Address;

        _context.Branches.Update(branch);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

