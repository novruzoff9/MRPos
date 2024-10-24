using Shared.Services;

namespace Organization.Application.Tables.Queries.GetTablesQuery;

public record GetTables : IRequest<List<Table>>;

public class GetTablesQueryHandler : IRequestHandler<GetTables, List<Table>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISharedIdentityService _identityService;

    public GetTablesQueryHandler(IApplicationDbContext context, ISharedIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<List<Table>> Handle(GetTables request, CancellationToken cancellationToken)
    {
        var tables = await _context.Tables.Where(x=>x.BranchId == _identityService.GetBranchId).ToListAsync(cancellationToken);
        return tables;
    }
}

