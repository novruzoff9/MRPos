using Mapster;
using Shared.Interfaces;
using Store.Application.Common.Models.Table;

namespace Store.Application.Features.Tables;

public record GetTablesQuery : IRequest<List<TableReturnDto>>;

public class GetTablesQueryHandler(
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetTablesQuery, List<TableReturnDto>>
{
    public async Task<List<TableReturnDto>> Handle(GetTablesQuery request, CancellationToken cancellationToken)
    {
        string branchId = identityService.GetBranchId;
        decimal serviceFee = dbContext.Branches
            .Where(b => b.Id == branchId)
            .Select(b => b.ServiceFee)
            .FirstOrDefault();
        var query = dbContext.Tables.AsQueryable();
        if (!string.IsNullOrEmpty(branchId))
            query = query.Where(t => t.BranchId == branchId);
        using var scope = new MapContextScope();
        scope.Context.Parameters["ServiceFee"] = serviceFee;

        var tables = await query
            .ProjectToType<TableReturnDto>()
            .ToListAsync(cancellationToken);
        return tables;
    }
}