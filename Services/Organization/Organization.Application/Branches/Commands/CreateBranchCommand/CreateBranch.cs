using Organization.Application.Common.Services;
using Organization.Domain.ValueObjects;
using Shared.Services;

namespace Organization.Application.Branches.Commands.CreateBranchCommand;

public record CreateBranch(bool Is24Hour, decimal ServiceFee, TimeOnly Opening, TimeOnly Closing, Address Address) : IRequest<bool>;

public class CreateBranchCommandHandler : IRequestHandler<CreateBranch, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public CreateBranchCommandHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<bool> Handle(CreateBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateBranch));
        GenerateOrganizationId IdGenerator = new GenerateOrganizationId(_context);

        var branch = new Branch
        {
            Id = await IdGenerator.GenerateUniqueBranchIdAsync(),
            CompanyId = _identityService.GetCompanyId,
            ServiceFee = request.ServiceFee,
            Is24Hour = request.Is24Hour,
            Opening = request.Opening,
            Closing = request.Closing,
            Created = DateTime.UtcNow,
            Address = request.Address
        };

        await _context.Branches.AddAsync(branch, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}

