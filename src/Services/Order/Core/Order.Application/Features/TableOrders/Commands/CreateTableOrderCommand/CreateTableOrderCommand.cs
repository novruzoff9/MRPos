using Order.Application.Common.Models.OrderItem;
using Shared.Interfaces;

namespace Order.Application.Features.TableOrders;

public record CreateTableOrderCommand(string TableNumber, string TableId, decimal Deposit,
    decimal ServicePercentage, ICollection<CreateOrderItemDto> Items) : IRequest<bool>;

public class CreateTableOrderCommandHandler(
    IMapper mapper,
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateTableOrderCommand, bool>
{
    public async Task<bool> Handle(CreateTableOrderCommand request, CancellationToken cancellationToken)
    {
        string branchId = identityService.GetBranchId;
        string waiterId = identityService.GetUserId;
        var orderItems = mapper.Map<ICollection<OrderItem>>(request.Items);
        var tableorder = new TableOrder(branchId, request.TableNumber, identityService.GetCompanyId, request.Deposit, request.ServicePercentage, waiterId, orderItems);

        await dbContext.Orders.AddAsync(tableorder, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

