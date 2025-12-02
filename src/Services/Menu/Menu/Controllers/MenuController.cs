using Catalog.Domain.Entities;
using Menu.Data;
using Menu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace Menu.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController(IHttpClientFactory httpClientFactory, MenuDbContext context, IIdentityService identityService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsOfBranch()
    {
        var branchId = identityService.GetBranchId;
        if (branchId == null)
        {
            return BadRequest("Branch ID is not available.");
        }
        var products = await context.MenuItems.Where(x => x.BranchId == branchId).ToListAsync();
        var result = Shared.ResultTypes.Response<List<MenuItem>>.Success(products, 200);
        return Ok(result);
    }

    [HttpGet("{branchId}")]
    public async Task<IActionResult> GetProductsOfBranch(string branchId)
    {
        if (branchId == null)
        {
            return BadRequest("Branch ID is not available.");
        }
        var products = await context.MenuItems.Where(x => x.BranchId == branchId).ToListAsync();
        var result = Shared.ResultTypes.Response<List<MenuItem>>.Success(products, 200);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToMenu(MenuItemCreateDto command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("ProductApiClient");
        //var token = await _tokenService.GetTokenAsync();

        //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);

        var productResponse = await client.GetAsync($"http://localhost:5002/api/products/{command.ProductId}");

        if (!productResponse.IsSuccessStatusCode)
        {
            return NotFound("Məhsul tapılmadı");
        }

        /*var branchResponse = await client.GetAsync($"http://localhost:5005/api/Branches/{_identityService.GetBranchId}");

        if (!branchResponse.IsSuccessStatusCode)
        {
            return NotFound("Filial tapılmadı");
        }*/

        var product = await productResponse.Content.ReadFromJsonAsync<Product>();
        //var branch = await branchResponse.Content.ReadFromJsonAsync<Branch>();

        //var categoryResponse = await client.GetAsync($"http://localhost:5002/api/categories/{product.CategoryId}");
        //var category = await categoryResponse.Content.ReadFromJsonAsync<Category>();

        /*if(category.CompanyId != branch.CompanyId)
        {
            return BadRequest("Bu şirkətə aid olmayan məhsul artırmağa çalışırsınız");
        }*/

        var menuItem = new MenuItem
        {
            ProductId = command.ProductId,
            BranchId = identityService.GetBranchId,
            ProductName = product.Name,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Price = product.Price
        };

        await context.MenuItems.AddAsync(menuItem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(menuItem);
    }
}
