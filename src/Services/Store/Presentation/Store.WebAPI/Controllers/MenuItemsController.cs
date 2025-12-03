using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;
using Store.Application.Common.Interfaces;
using Store.Application.Common.Models.MenuItem;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemsController
    (IMenuItemService menuItemService): ControllerBase
{
    [HttpGet("{branchId}")]
    public async Task<IActionResult> GetAllItemsOfBranch(string branchId)
    {
        var menuItems = await menuItemService.GetAllItemsOfBranchAsync(branchId);
        var response = Response<List<MenuItemReturnDto>>.Success(menuItems, 200);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(MenuItemCreateDto createDto)
    {
        var menuItem = await menuItemService.AddItemAsync(createDto);
        var response = Response<MenuItemReturnDto>.Success(menuItem, 201);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        var menuItem = await menuItemService.DeleteItemAsync(id);
        var response = Response<MenuItemReturnDto>.Success(menuItem, 200);
        return Ok(response);
    }
}
