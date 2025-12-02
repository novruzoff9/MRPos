using Shared.Interfaces;
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
            .AsNoTracking()
            .ProjectToType<TableReturnDto>()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        //TODO: Check branchId with table.BranchId and null
        return table;
    }
}
