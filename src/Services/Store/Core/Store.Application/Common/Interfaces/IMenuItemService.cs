using Store.Application.Common.Models.MenuItem;
using Store.Application.Common.Models.Product;

namespace Store.Application.Common.Interfaces;

public interface IMenuItemService
{
    Task<bool> AddItemAsync(MenuItemDto createDto);
    Task<bool> AddMultipleItemsAsync(ICollection<string> ids);
    Task<bool> DeleteItemAsync(string id);
    Task<List<MenuItemReturnDto>> GetAllItemsOfBranchAsync(string branchId);
    Task<List<ProductReturnDto>> GetAvailableProducts(string branchId);
}

public class MenuItemService(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IIdentityService identityService
    ) : IMenuItemService
{
    public async Task<bool> AddItemAsync(MenuItemDto menuItemDto)
    {
        bool isExsists = await dbContext.Products.AnyAsync(x => x.Id == menuItemDto.ProductId);
        if (!isExsists)
            throw new NotFoundException($"Product with ProductId {menuItemDto.ProductId} not found");
        bool isAlreadyInMenu = await dbContext.MenuItems.AnyAsync(x => x.ProductId == menuItemDto.ProductId && x.BranchId == identityService.GetBranchId);
        if (isAlreadyInMenu)
            throw new ConflictException($"Product with ProductId {menuItemDto.ProductId} is already in menu");
        MenuItemCreateDto createDto = new(
            identityService.GetBranchId,
            identityService.GetCompanyId,
            menuItemDto.ProductId
        );
        var menuItem = mapper.Map<MenuItem>(createDto);
        await dbContext.MenuItems.AddAsync(menuItem);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddMultipleItemsAsync(ICollection<string> ids)
    {
        var branchId = identityService.GetBranchId;
        var companyId = identityService.GetCompanyId;

        var existingProductIds = await dbContext.Products
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        var notFoundIds = ids.Except(existingProductIds).ToList();
        if (notFoundIds.Any())
            throw new NotFoundException(
                $"Products not found: {string.Join(", ", notFoundIds)}");

        var alreadyInMenuIds = await dbContext.MenuItems
            .Where(x => ids.Contains(x.ProductId) && x.BranchId == branchId)
            .Select(x => x.ProductId)
            .ToListAsync();

        if (alreadyInMenuIds.Any())
            throw new ConflictException(
                $"Products already in menu: {string.Join(", ", alreadyInMenuIds)}");

        var menuItems = ids.Select(id =>
            mapper.Map<MenuItem>(
                new MenuItemCreateDto(branchId, companyId, id)
            )).ToList();

        await dbContext.MenuItems.AddRangeAsync(menuItems);

        return await dbContext.SaveChangesAsync() > 0;
    }


    public async Task<bool> DeleteItemAsync(string id)
    {
        var menuItem = await dbContext.MenuItems.FirstOrDefaultAsync(mi => mi.Id == id);
        if (menuItem == null)
            throw new NotFoundException($"MenuItem with Id {id} not found");
        dbContext.MenuItems.Remove(menuItem);
        return await dbContext.SaveChangesAsync() > 0;
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

    public async Task<List<ProductReturnDto>> GetAvailableProducts(string branchId)
    {
        var menuItemIds = await dbContext.MenuItems
            .Where(x=>x.BranchId == branchId)
            .Select(x => x.ProductId)
            .ToListAsync();
        var products = await dbContext.Products
            .Where(x => !menuItemIds.Contains(x.Id))
            .ProjectToType<ProductReturnDto>()
            .ToListAsync();
        return products;
    }
}
