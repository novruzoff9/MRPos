namespace Store.Application.Features.Branches;

public record UpdateBranchServiceFeeCommand(string? BranchId, decimal ServiceFee) : IRequest<bool>;

public class UpdateBranchServiceFeeCommandHandler(
    IApplicationDbContext dbContext,
    IIdentityService identityService
    ) : IRequestHandler<UpdateBranchServiceFeeCommand, bool>
{
    public async Task<bool> Handle(UpdateBranchServiceFeeCommand request, CancellationToken cancellationToken)
    {
        string companyId = identityService.GetCompanyId;
        string? branchId = request.BranchId;
        if (string.IsNullOrEmpty(branchId))
            branchId = identityService.GetBranchId;

        var branch = await dbContext.Branches
            .FirstOrDefaultAsync(x => x.Id == branchId, cancellationToken);

        if (branch is null)
            throw new NotFoundException($"Branch not found with ID: {branchId}");

        if (branch.CompanyId != companyId)
            throw new ForbiddenAccessException("You do not have permission to update this branch.");

        branch.UpdateServiceFee(request.ServiceFee);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
