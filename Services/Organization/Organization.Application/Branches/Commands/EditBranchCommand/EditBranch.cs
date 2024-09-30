using Organization.Domain.ValueObjects;
using System.Runtime.InteropServices;

namespace Catalog.Application.Branches.Commands.EditBranchCommand;

public record EditBranch(string Id, string CompanyId, bool Is24Hour, decimal ServiceFee, TimeOnly Opening, TimeOnly Closing, Address Address) : IRequest<bool>;

public class EditBranchCommandHandler : IRequestHandler<EditBranch, bool>
{
    private readonly IApplicationDbContext _context;

    public EditBranchCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.CompanyId, nameof(request));
        Guard.Against.Negative(request.ServiceFee, nameof(request));

        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (branch == null) { return false; }

        branch.CompanyId = request.CompanyId;
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

