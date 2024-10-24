using Catalog.Application.Categories.Commands.DeleteCategoryCommand;
using Catalog.Application.Categories.Commands.EditCategoryCommand;
using Catalog.Application.Categories.Queries.GetCategoryQuery;
using Catalog.Application.Products.Commands.CreateProductCommand;
using Catalog.Application.Products.Commands.DeleteProductCommand;
using Catalog.Application.Products.Commands.EditProductCommand;
using Catalog.Application.Products.Queries.GetProductQuery;
using Catalog.Application.Products.Queries.GetProductsByCategoryQuery;
using Catalog.Application.Products.Queries.GetProductsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ProductsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateProduct command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetProducts());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Sender.Send(new GetProduct(id));
        return Ok(result);
    }

    [HttpGet("ByCategoryId/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await Sender.Send(new GetProductsByCategory(categoryId));
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditProduct command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Sender.Send(new DeleteProduct(id));
        return Ok(result);
    }
}
