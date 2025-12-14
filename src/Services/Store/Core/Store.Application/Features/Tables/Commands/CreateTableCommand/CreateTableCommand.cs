namespace Store.Application.Features.Tables;

public record CreateTableCommand(string Name, int? Capacity, decimal? Deposit = null)
    : IRequest<bool>;

public class CreateTableCommandHandler(
    IMapper mapper,
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateTableCommand, bool>
{
    public async Task<bool> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        string branchId = identityService.GetBranchId;
        var tableExsists = await dbContext.Tables
            .AnyAsync(x => x.Name == request.Name && x.BranchId == branchId, cancellationToken);
        if (tableExsists)
            throw new ConflictException($"A table with the name '{request.Name}' already exists.");
        
        var table = new Table(
            name: request.Name,
            capacity: request.Capacity,
            branchId: branchId,
            deposit: request.Deposit
        );

        await dbContext.Tables.AddAsync(table, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
