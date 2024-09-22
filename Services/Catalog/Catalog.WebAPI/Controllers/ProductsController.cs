using Catalog.Application.Categories.Commands.DeleteCategoryCommand;
using Catalog.Application.Categories.Commands.EditCategoryCommand;
using Catalog.Application.Categories.Queries.GetCategoryQuery;
using Catalog.Application.Products.Commands.CreateProductCommand;
using Catalog.Application.Products.Queries.GetProductsQuery;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        var result = await Sender.Send(new GetCategory(id));
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditCategory command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Sender.Send(new DeleteCategory(id));
        return Ok(result);
    }
}
