using Microsoft.AspNetCore.SignalR;
using Order.WebAPI.Models;

namespace Order.WebAPI.Hubs
{
    public class OrderHub : Hub
    {
        public async Task OrderUpdated(List<OrderItem> orderItems, string tableNumber)
        {
            await Clients.All.SendAsync("OrderUpdated", orderItems, tableNumber);
        }

        public async Task OrderPrepared(OrderItem orderItem, string tableNumber)
        {
            await Clients.All.SendAsync("OrderPrepared", orderItem, tableNumber);
        }

        public async Task CallWaiter(string tableNumber)
        {
            await Clients.All.SendAsync("CallWaiter", tableNumber);
        }
    }
}
