using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Common.Models.Order;
using Order.Application.Features.TableOrders;
using Shared.ResultTypes;

namespace Order.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await sender.Send(new GetTableOrdersQuery());
        var response = Response<ICollection<ReturnOrderDto>>.Success(orders, 200);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(string id)
    {
        var order = await sender.Send(new GetTableOrderQuery(id));
        var response = Response<ReturnOrderDto>.Success(order, 200);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateTableOrderCommand command)
    {
        var result = await sender.Send(command);
        var response = Response<bool>.Success(result, 201);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{orderId}/items")]
    public async Task<IActionResult> AddItems(string orderId, AddItemsToOrderCommand command)
    {
        command = command with { OrderId = orderId };
        var result =  await sender.Send(command);
        var response = Response<bool>.Success(result, 200);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{orderId}/complete")]
    public async Task<IActionResult> CompleteOrder(string orderId)
    {
        var result = await sender.Send(new CompleteOrderCommand(orderId));
        var response = Response<bool>.Success(result, 200); 
        return StatusCode(response.StatusCode, response);
    }
}
