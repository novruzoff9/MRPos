using Shared.Interfaces;
using Store.Domain.ValueObjects;

namespace Store.Application.Features.Branches;

public record CreateBranchCommand(string Name, string PhoneNumber, string? Description,
        bool Is24Hour, TimeOnly? Opening, TimeOnly? Closing,
        decimal ServiceFee, Address Address) : IRequest<bool>;

public class CreateBranchCommandHandler(
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateBranchCommand, bool>
{
    public async Task<bool> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        string companyId = identityService.GetCompanyId;
        var branch = new Branch(
            request.Name,
            request.PhoneNumber,
            request.Description,
            request.Is24Hour,
            request.Opening,
            request.Closing,
            request.ServiceFee,
            request.Address,
            companyId
            );
        await dbContext.Branches.AddAsync(branch, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}