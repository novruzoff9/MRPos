using Order.Application.Common.Models.OrderItem;
using Shared.Interfaces;

namespace Order.Application.Features.TableOrders;

public record CreateTableOrderCommand(string BranchId, string TableNumber, decimal Deposit,
    decimal ServicePercentage, string? WaiterId, ICollection<CreateOrderItemDto> Items) : IRequest<bool>;

public class CreateTableOrderCommandHandler(
    IMapper mapper,
    IIdentityService identityService,
    IApplicationDbContext dbContext
    ) : IRequestHandler<CreateTableOrderCommand, bool>
{
    public async Task<bool> Handle(CreateTableOrderCommand request, CancellationToken cancellationToken)
    {
        var orderItems = mapper.Map<ICollection<OrderItem>>(request.Items); 
        var tableorder = new TableOrder(request.BranchId, request.TableNumber, identityService.GetCompanyId, request.Deposit, request.ServicePercentage, request.WaiterId, orderItems);

        await dbContext.Orders.AddAsync(tableorder, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}

