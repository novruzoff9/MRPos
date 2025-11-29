namespace Organization.Application.Tables.Queries.GetTableQuery;

public record GetTable(string TableNumber, string BranhcId) : IRequest<Table>;

public class GetTableQueryHandler : IRequestHandler<GetTable, Table>
{
    private readonly IApplicationDbContext _context;

    public GetTableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Table> Handle(GetTable request, CancellationToken cancellationToken)
    {
        var table = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == request.TableNumber && x.BranchId == request.BranhcId, cancellationToken);
        return table;
    }
}

