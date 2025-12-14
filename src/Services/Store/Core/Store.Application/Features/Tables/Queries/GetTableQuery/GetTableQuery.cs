using Store.Application.Common.Models.Table;

namespace Store.Application.Features.Tables;

public record GetTableQuery(string Id) : IRequest<TableReturnDto>;

public class GetTableQueryHandler(
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<GetTableQuery, TableReturnDto>
{
    public async Task<TableReturnDto> Handle(GetTableQuery request, CancellationToken cancellationToken)
    {
        string branchId = identityService.GetBranchId;
        var table = await dbContext.Tables
            .Where(t => t.Id == request.Id && t.BranchId == branchId)
            .ProjectToType<TableReturnDto>()
            .FirstOrDefaultAsync(cancellationToken);
        if (table == null)
            throw new NotFoundException($"Table not found with ID: {request.Id}");
        return table;
    }
}