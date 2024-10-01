using Organization.Domain.Entities;

namespace Organization.Application.Tables.Commands.EditTableCommand;

public record EditTable(string Id, string BranchId, string TableNumber, decimal Deposit) : IRequest<bool>;

public class EditTableCommandHandler : IRequestHandler<EditTable, bool>
{
    private readonly IApplicationDbContext _context;

    public EditTableCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditTable request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.BranchId, nameof(request));
        Guard.Against.NotFound(request.TableNumber, nameof(request));
        Guard.Against.Negative(request.Deposit, nameof(request));

        var table = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == request.Id, cancellationToken);

        if (table == null) { return false; }

        table.TableNumber = request.TableNumber;
        table.Deposit = request.Deposit;
        table.BranchId = request.BranchId;

        _context.Tables.Update(table);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

