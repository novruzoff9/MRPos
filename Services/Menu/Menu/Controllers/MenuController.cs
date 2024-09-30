using Catalog.Domain.Entities;
using Menu.Data;
using Menu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly MenuDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public MenuController(IHttpClientFactory httpClientFactory, MenuDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToMenu(int productId, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("ProductApiClient");
        var productResponse = await client.GetAsync($"http://localhost:5002/api/products/{productId}");

        if (!productResponse.IsSuccessStatusCode)
        {
            return NotFound("Məhsul tapılmadı");
        }

        var product = await productResponse.Content.ReadFromJsonAsync<Product>();

        var menuItem = new MenuItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Price = product.Price
        };

        await _context.MenuItems.AddAsync(menuItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(menuItem);
    }
}
