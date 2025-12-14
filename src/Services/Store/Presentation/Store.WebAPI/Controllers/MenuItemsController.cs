using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Shared.ResultTypes;
using Store.Application.Common.Interfaces;
using Store.Application.Common.Models.MenuItem;
using Store.Application.Common.Models.Product;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemsController(
    IMenuItemService menuItemService,
    IIdentityService identityService
    ): ControllerBase
{
    [HttpGet("{branchId}")]
    public async Task<IActionResult> GetAllItemsOfBranch(string branchId)
    {
        var menuItems = await menuItemService.GetAllItemsOfBranchAsync(branchId);
        var response = Response<ICollection<MenuItemReturnDto>>.Success(menuItems, 200);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(MenuItemDto createDto)
    {
        var menuItem = await menuItemService.AddItemAsync(createDto);
        var response = Response<bool>.Success(menuItem, 201);
        return Ok(response);
    }

    [HttpPost("multiple")]
    public async Task<IActionResult> AddMultipleItems(ICollection<string> createDtos)
    {
        var menuItem = await menuItemService.AddMultipleItemsAsync(createDtos);
        var response = Response<bool>.Success(menuItem, 201);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItemsOfBranch()
    {
        string branchId = identityService.GetBranchId;
        var menuItems = await menuItemService.GetAllItemsOfBranchAsync(branchId);
        var response = Response<ICollection<MenuItemReturnDto>>.Success(menuItems, 200);
        return Ok(response);
    }

    [HttpGet("available-products")]
    public async Task<IActionResult> GetAvailableProducts()
    {
        string branchId = identityService.GetBranchId;
        var products = await menuItemService.GetAvailableProducts(branchId);
        var response = Response<ICollection<ProductReturnDto>>.Success(products, 200);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        var menuItem = await menuItemService.DeleteItemAsync(id);
        var response = Response<bool>.Success(menuItem, 200);
        return Ok(response);
    }
}
