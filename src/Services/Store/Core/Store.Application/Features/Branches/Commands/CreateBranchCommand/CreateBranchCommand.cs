using Store.Application.Common.Models.Branch;
using Store.Domain.ValueObjects;

namespace Store.Application.Features.Branches;

public record CreateBranchCommand(string Name, string PhoneNumber, string? Description,
        bool Is24Hour, TimeOnly? Opening, TimeOnly? Closing,
        decimal ServiceFee, AddressDto Address) : IRequest<bool>;

public class CreateBranchCommandHandler(
    IMapper mapper,
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateBranchCommand, bool>
{
    public async Task<bool> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        string companyId = identityService.GetCompanyId;
        var existingBranch = await dbContext.Branches
            .FirstOrDefaultAsync(b => b.Name == request.Name && b.CompanyId == companyId, cancellationToken);
        if (existingBranch != null) 
            throw new ConflictException("A branch with the same name already exists in your company.");
        var address = mapper.Map<Address>(request.Address);
        var branch = new Branch(
            request.Name,
            request.PhoneNumber,
            request.Description,
            request.Is24Hour,
            request.Opening,
            request.Closing,
            request.ServiceFee,
            address,
            companyId
            );
        await dbContext.Branches.AddAsync(branch, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
