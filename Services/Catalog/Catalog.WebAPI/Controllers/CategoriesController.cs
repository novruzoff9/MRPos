using Catalog.Application.Categories.Commands.CreateCategoryCommand;
using Catalog.Application.Categories.Commands.DeleteCategoryCommand;
using Catalog.Application.Categories.Commands.EditCategoryCommand;
using Catalog.Application.Categories.Queries.GetCategoriesQuery;
using Catalog.Application.Categories.Queries.GetCategoryQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : BaseController
{

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategory command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new GetCategories());
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
