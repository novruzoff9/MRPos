using Store.Application.Common.Models.Category;

namespace Store.Application.Common.Mappings;

internal class CategoryMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryReturnDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ProductCount, src => src.Products.Count);
    }
}
