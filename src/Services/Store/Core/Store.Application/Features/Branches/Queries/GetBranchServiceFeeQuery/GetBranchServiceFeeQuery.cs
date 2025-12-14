namespace Store.Application.Features.Branches;

public record GetBranchServiceFeeQuery() : IRequest<decimal>;

public class GetBranchServiceFeeQueryHandler(
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetBranchServiceFeeQuery, decimal>
{
    public async Task<decimal> Handle(GetBranchServiceFeeQuery request, CancellationToken cancellationToken)
    {
        string branchId = identityService.GetBranchId;
        var branch = await dbContext.Branches
            .Where(x => x.Id == branchId)
            .Select(x => new { x.ServiceFee })
            .FirstOrDefaultAsync(cancellationToken);

        if (branch is null)
            throw new NotFoundException($"Branch not found with ID: {branchId}");

        return branch.ServiceFee;
    }
}
