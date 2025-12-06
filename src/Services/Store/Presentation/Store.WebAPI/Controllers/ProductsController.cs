using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;
using Store.Application.Common.Models.Product;
using Store.Application.Features.Products;

namespace Store.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(
    ISender sender
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 204);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await sender.Send(new GetProductsQuery());
        var response = Response<ICollection<ProductReturnDto>>.Success(result, 200);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await sender.Send(new GetProductQuery(id));
        var response = Response<ProductReturnDto>.Success(result, 200);
        return Ok(response);
    }

    [HttpGet("ByCategoryId/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(string categoryId)
    {
        var result = await sender.Send(new GetProductsByCategoryQuery(categoryId));
        var response = Response<ICollection<ProductReturnDto>>.Success(result, 200);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await sender.Send(new DeleteProductCommand(id));
        var response = Response<bool>.Success(result, 204);
        return Ok(response);
    }
}
