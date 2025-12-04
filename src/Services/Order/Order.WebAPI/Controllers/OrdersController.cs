//using AutoMapper;
//using Menu.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Order.WebAPI.DTOs;
//using Order.WebAPI.Hubs;
//using Order.WebAPI.Models;

//namespace Order.WebAPI.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class OrdersController(IIdentityService identityService, OrderDbContext context, IMapper mapper, IHubContext<OrderHub> hubContext, IHttpClientFactory httpClientFactory) : ControllerBase
//{
//    [HttpPost]
//    public IActionResult CreateOrder(OrderDTO request)
//    {
//        var client = httpClientFactory.CreateClient("BranchApiClient");
//        var response = client.GetAsync($"http://localhost:5005/api/Branches/branch-serviceFee/{request.BranchId}").Result;
//        if (context.Orders
//            .Where(x => x.BranchId == request.BranchId && x.TableNumber == request.TableNumber && x.Completed == false)
//            .Any())
//        {
//            return BadRequest("Bu masa üçün tamamlanmamış sifariş var");
//        }
//        var order = new TableOrder();
//        order.Opened = DateTime.Now;
//        order.WaiterId = identityService.GetUserId;
//        order.BranchId = request.BranchId;
//        order.TableNumber = request.TableNumber;
//        order.ServiceFee = decimal.Parse(response.Content.ReadAsStringAsync().Result);
//        order.Completed = false;
//        context.Orders.Add(order);
//        context.SaveChanges();
//        return Ok(order);
//    }

//    [HttpPost("add-item")]
//    public async Task<IActionResult> AddItemToOrder(AddOrderItemDto request)
//    {
//        var order = await context.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.OrderId);
//        if (order == null)
//        {
//            return NotFound("Order not found.");
//        }

//        foreach (var item in request.Items)
//        {
//            var existingItem = order.Items.FirstOrDefault(x => x.ProductName == item.ProductName);
//            if (existingItem != null)
//            {
//                existingItem.Quantity += item.Quantity;
//            }
//            else
//            {
//                OrderItem newItem = new OrderItem
//                {
//                    OrderId = order.Id,
//                    Price = item.Price,
//                    ProductName = item.ProductName,
//                    Quantity = item.Quantity
//                };
//                await context.OrderItems.AddAsync(newItem);
//            }
//        }

//        await context.SaveChangesAsync();

//        //await _hubContext.Clients.All.SendAsync("OrderUpdated", order.Id);

//        return Ok("Order updated successfully!");
//    }

//    [HttpPost("complete/{id}")]
//    public IActionResult CompleteOrder(int id)
//    {
//        var order = context.Orders.FirstOrDefault(x => x.Id == id);
//        if (order == null)
//        {
//            return NotFound("Order not found.");
//        }
//        order.Completed = true;
//        order.Closed = DateTime.Now;
//        context.SaveChanges();
//        return Ok("Order completed successfully!");
//    }

//    [HttpGet("check-status")]
//    public IActionResult CheckStatus(string branchId, string TableNumber)
//    {
//        var order = context.Orders
//            .FirstOrDefault(x => x.BranchId == branchId && x.TableNumber == TableNumber && x.Completed == false);
//        return Ok(new { hasOpenOrder = (order != null), orderId = order?.Id ?? 0 });
//    }

//    [HttpGet("{id}")]
//    public IActionResult OrderDetails(int id)
//    {
//        var order = context.Orders.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
//        if (order == null)
//        {
//            return NotFound("Order not found.");
//        }
//        foreach (var item in order.Items)
//        {
//            item.Order = null;
//        }
//        return Ok(order);
//    }

//    [HttpGet("completed")]
//    public IActionResult CompletedOrders()
//    {
//        var orders = context.Orders.Include(x => x.Items).Where(x => x.Completed == true && x.BranchId == identityService.GetBranchId).ToList();
//        foreach (var order in orders)
//        {
//            foreach (var item in order.Items)
//            {
//                item.Order = null;
//            }
//        }
//        var response = Shared.ResultTypes.Response<List<TableOrder>>.Success(orders, 200);
//        return Ok(response);
//    }

//    [HttpGet("active")]
//    public IActionResult ActiveOrders()
//    {
//        var orders = context.Orders.Include(x=>x.Items).Where(x => x.Completed == false && x.BranchId == identityService.GetBranchId).ToList();
//        foreach (var order in orders)
//        {
//            foreach (var item in order.Items)
//            {
//                item.Order = null;
//            }
//        }
//        var response = Shared.ResultTypes.Response<List<TableOrder>>.Success(orders, 200);
//        return Ok(response);
//    }
    
//    [HttpGet("table-status")]
//    public IActionResult TableStatus(string branchId, string tableNumber)
//    {
//        var order = context.Orders.Include(x=>x.Items).FirstOrDefault(x => x.BranchId == branchId && x.TableNumber == tableNumber && x.Completed == false);
//        foreach (var item in order.Items)
//        {
//            item.Order = null;
//        }
//        return Ok(order);
//    }
    
//}
