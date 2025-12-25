using Store.Application.Common.Models.Table;
using Store.Application.Features.Tables;

namespace Store.Application.Common.Mappings;

internal class TableMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Table, TableReturnDto>()
            .ConstructUsing(src => new TableReturnDto(
                src.Id, src.Name, src.Capacity, src.TableStatus.ToString(), src.Deposit
            ))
           .Map(dest => dest.ServiceFee, src => MapContext.Current.Parameters["ServiceFee"]);
    }
}