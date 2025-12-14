using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.General;
using Shared.ResultTypes;
using Store.Application.Common.Interfaces;
using Store.Application.Common.Models.Category;
using Store.Application.Features.Categories;
using Store.Domain.Entities;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(
    ISender sender,
    ILookupService lookupService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await sender.Send(new GetCategoriesQuery());
        var response = Response<ICollection<CategoryReturnDto>>.Success(categories, 200);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id)
    {
        var category = await sender.Send(new GetCategoryQuery(id));
        var response = Response<CategoryReturnDto>.Success(category, 200);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }
    [HttpGet("company/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoriesByCompanyId(string id)
    {
        var result = await sender.Send(new GetCategoriesofCompany(id));
        var response = Response<ICollection<CategoryReturnDto>>.Success(result, 200);
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Edit(EditCategoryCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await sender.Send(new DeleteCategoryCommand(id));
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> GetCategoriesLookup()
    {
        var lookup = await lookupService.GetLookupAsync<Category, string>();
        var response = Response<List<LookupDto<string>>>.Success(lookup, 200);
        return Ok(response);
    }
}
