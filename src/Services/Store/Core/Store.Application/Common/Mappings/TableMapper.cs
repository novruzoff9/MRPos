using Store.Application.Features.Tables;

namespace Store.Application.Common.Mappings;

internal class TableMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTableCommand, Table>()
            .ConstructUsing(src => new Table(
                src.Name,
                src.Capacity,
                src.BranchId,
                src.Deposit
            ));
    }
}
