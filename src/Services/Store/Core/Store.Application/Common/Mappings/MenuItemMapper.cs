using Store.Application.Common.Models.MenuItem;

namespace Store.Application.Common.Mappings;

public class MenuItemMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MenuItemCreateDto, MenuItem>()
              .MapToConstructor(true);
        config.NewConfig<MenuItem, MenuItemReturnDto>()
              .Map(dest => dest.CategoryName, src => src.Product.Category.Name)
              .Map(dest => dest.Name, src => src.Product.Name)
              .Map(dest => dest.Price, src => src.Product.Price.ToString("F2"))
              .Map(dest => dest.Description, src => src.Product.Description)
              .MapToConstructor(true);
    }
}
