using AutoMapper;
using Azure.Core;
using Menu.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.WebAPI.DTOs;
using Order.WebAPI.Hubs;
using Order.WebAPI.Models;
using Shared.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Order.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ISharedIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OrderDbContext _context;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrdersController(ISharedIdentityService identityService, OrderDbContext context, IMapper mapper, IHubContext<OrderHub> hubContext, IHttpClientFactory httpClientFactory)
    {
        _identityService = identityService;
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public IActionResult CreateOrder(OrderDTO request)
    {
        var client = _httpClientFactory.CreateClient("BranchApiClient");
        var response = client.GetAsync($"http://localhost:5005/api/Branches/branch-serviceFee/{request.BranchId}").Result;
        if (_context.Orders
            .Where(x => x.BranchId == request.BranchId && x.TableNumber == request.TableNumber && x.Completed == false)
            .Any())
        {
            return BadRequest("Bu masa üçün tamamlanmamış sifariş var");
        }
        var order = new TableOrder();
        order.Opened = DateTime.Now;
        order.WaiterId = _identityService.GetUserId;
        order.BranchId = request.BranchId;
        order.TableNumber = request.TableNumber;
        order.ServiceFee = decimal.Parse(response.Content.ReadAsStringAsync().Result);
        order.Completed = false;
        _context.Orders.Add(order);
        _context.SaveChanges();
        return Ok(order);
    }

    [HttpPost("add-item")]
    public async Task<IActionResult> AddItemToOrder(AddOrderItemDto request)
    {
        var order = await _context.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.OrderId);
        if (order == null)
        {
            return NotFound("Order not found.");
        }

        foreach (var item in request.Items)
        {
            var existingItem = order.Items.FirstOrDefault(x => x.ProductName == item.ProductName);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                OrderItem newItem = new OrderItem
                {
                    OrderId = order.Id,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                };
                await _context.OrderItems.AddAsync(newItem);
            }
        }

        await _context.SaveChangesAsync();

        //await _hubContext.Clients.All.SendAsync("OrderUpdated", order.Id);

        return Ok("Order updated successfully!");
    }

    [HttpPost("complete/{id}")]
    public IActionResult CompleteOrder(int id)
    {
        var order = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (order == null)
        {
            return NotFound("Order not found.");
        }
        order.Completed = true;
        order.Closed = DateTime.Now;
        _context.SaveChanges();
        return Ok("Order completed successfully!");
    }

    [HttpGet("check-status")]
    public IActionResult CheckStatus(string branchId, string TableNumber)
    {
        var order = _context.Orders
            .FirstOrDefault(x => x.BranchId == branchId && x.TableNumber == TableNumber && x.Completed == false);
        return Ok(new { hasOpenOrder = (order != null), orderId = order?.Id ?? 0 });
    }

    [HttpGet("{id}")]
    public IActionResult OrderDetails(int id)
    {
        var order = _context.Orders.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
        if (order == null)
        {
            return NotFound("Order not found.");
        }
        foreach (var item in order.Items)
        {
            item.Order = null;
        }
        return Ok(order);
    }

    [HttpGet("completed")]
    public IActionResult CompletedOrders()
    {
        var orders = _context.Orders.Include(x => x.Items).Where(x => x.Completed == true && x.BranchId == _identityService.GetBranchId).ToList();
        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                item.Order = null;
            }
        }
        var response = Shared.ResultTypes.Response<List<TableOrder>>.Success(orders, 200);
        return Ok(response);
    }

    [HttpGet("active")]
    public IActionResult ActiveOrders()
    {
        var orders = _context.Orders.Include(x=>x.Items).Where(x => x.Completed == false && x.BranchId == _identityService.GetBranchId).ToList();
        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                item.Order = null;
            }
        }
        var response = Shared.ResultTypes.Response<List<TableOrder>>.Success(orders, 200);
        return Ok(response);
    }
    
    [HttpGet("table-status")]
    public IActionResult TableStatus(string branchId, string tableNumber)
    {
        var order = _context.Orders.Include(x=>x.Items).FirstOrDefault(x => x.BranchId == branchId && x.TableNumber == tableNumber && x.Completed == false);
        foreach (var item in order.Items)
        {
            item.Order = null;
        }
        return Ok(order);
    }
    
}
