namespace Organization.Application.Tables.Commands.DeleteTableCommand;

public record DeleteTable(string Id) : IRequest<bool>;

public class DeleteTableCommandHandler : IRequestHandler<DeleteTable, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTableCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTable request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var table = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == request.Id, cancellationToken);
        if (table == null) { return false; }
        _context.Tables.Remove(table);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

