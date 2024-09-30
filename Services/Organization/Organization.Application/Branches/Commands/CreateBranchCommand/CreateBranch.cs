using Organization.Application.Common.Services;
using Organization.Domain.ValueObjects;

namespace Organization.Application.Branches.Commands.CreateBranchCommand;

public record CreateBranch(string CompanyId, bool Is24Hour, decimal ServiceFee, TimeOnly Opening, TimeOnly Closing, Address Address) : IRequest<bool>;

public class CreateBranchCommandHandler : IRequestHandler<CreateBranch, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateBranchCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateBranch request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateBranch));
        GenerateOrganizationId IdGenerator = new GenerateOrganizationId(_context);

        var branch = new Branch
        {
            Id = await IdGenerator.GenerateUniqueBranchIdAsync(),
            CompanyId = request.CompanyId,
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

