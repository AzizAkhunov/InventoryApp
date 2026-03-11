using Microsoft.AspNetCore.SignalR;

namespace InventoryApp.Server.Hubs
{
    public class DiscussionHub : Hub
    {
        public async Task JoinInventory(string inventoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, inventoryId);
        }

        public async Task LeaveInventory(string inventoryId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, inventoryId);
        }

        public async Task SendNotification(string userId, string inventoryTitle)
        {
            await Clients.User(userId).SendAsync("NewNotification", new
            {
                Title = inventoryTitle,
                message = "New discussion message"
            });
        }
    }
}
