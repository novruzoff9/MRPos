using Store.Application.Common.Models.MenuItem;

namespace Store.Application.Common.Interfaces;

public interface IMenuItemService
{
    Task<MenuItemReturnDto> AddItemAsync(MenuItemCreateDto createDto);
    Task<MenuItemReturnDto> DeleteItemAsync(string id);
    Task<List<MenuItemReturnDto>> GetAllItemsOfBranchAsync(string branchId);
}

public class MenuItemService(
    IMapper mapper,
    IApplicationDbContext dbContext
    ): IMenuItemService
{
    public async Task<MenuItemReturnDto> AddItemAsync(MenuItemCreateDto createDto)
    {
        var menuItem = mapper.Map<MenuItem>(createDto);
        await dbContext.MenuItems.AddAsync(menuItem);
        await dbContext.SaveChangesAsync();
        return mapper.Map<MenuItemReturnDto>(menuItem);

    }
    public async Task<MenuItemReturnDto> DeleteItemAsync(string id)
    {
        var menuItem = await dbContext.MenuItems.FirstOrDefaultAsync(mi => mi.Id == id);
        dbContext.MenuItems.Remove(menuItem);
        await dbContext.SaveChangesAsync();
        return mapper.Map<MenuItemReturnDto>(menuItem);
    }
    public async Task<List<MenuItemReturnDto>> GetAllItemsOfBranchAsync(string branchId)
    {
        var menuItems = await dbContext.MenuItems
            .Include(mi => mi.Product)
            .ThenInclude(p => p.Category)
            .Where(mi => mi.BranchId == branchId)
            .ProjectToType<MenuItemReturnDto>()
            .ToListAsync();

        return menuItems;
    }
}
