namespace Organization.Application.Tables.Commands.CreateTableCommand;

public record CreateTable(string BranchId, string TableNumber, decimal Deposit) : IRequest<bool>;

public class CreateTableCommandHandler : IRequestHandler<CreateTable, bool>
{
    private readonly IApplicationDbContext _context;

    public CreateTableCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateTable request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateTable));

        var table = new Table
        {
            TableNumber = request.TableNumber,
            Deposit = request.Deposit,
            BranchId = request.BranchId
        };

        await _context.Tables.AddAsync(table, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}

