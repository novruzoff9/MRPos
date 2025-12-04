namespace Store.Application.Features.Tables;

public record UpdateDepositCommand(string TableId, decimal Deposit) : IRequest<bool>;

public class UpdateDepositCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateDepositCommand, bool>
{
    public async Task<bool> Handle(UpdateDepositCommand request, CancellationToken cancellationToken)
    {
        var table = await dbContext.Tables.FirstOrDefaultAsync(t => t.Id == request.TableId, cancellationToken);
        if (table is null)
            throw new NotFoundException($"Table not found with ID: {request.TableId}");
        table.UpdateDeposit(request.Deposit);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
