namespace Organization.Application.Tables.Queries.GetTablesQuery;

public record GetTables : IRequest<List<Table>>;

public class GetTablesQueryHandler : IRequestHandler<GetTables, List<Table>>
{
    private readonly IApplicationDbContext _context;

    public GetTablesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Table>> Handle(GetTables request, CancellationToken cancellationToken)
    {
        var tables = await _context.Tables.ToListAsync(cancellationToken);
        return tables;
    }
}

