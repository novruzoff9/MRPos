namespace Store.Application.Features.Tables;

public record CreateTableCommand(string Name, int Capacity, string BranchId, decimal? Deposit = null) : IRequest<bool>;

public class CreateTableCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateTableCommand, bool>
{
    public async Task<bool> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var table = mapper.Map<Table>(request);
        await dbContext.Tables.AddAsync(table, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}