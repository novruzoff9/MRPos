using Store.Application.Common.Models.Product;

namespace Store.Application.Common.Mappings;

internal class ProductMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductReturnDto>()
            .ConstructUsing(src => new ProductReturnDto(
                src.Id,
                src.Name,
                src.Description,
                src.Status.ToString(),
                src.Category.Name,
                src.Price
            ));
    }
}
