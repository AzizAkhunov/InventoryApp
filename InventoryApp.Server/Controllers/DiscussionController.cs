using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly IHubContext<DiscussionHub> _hub;
        private readonly IDiscussionService _service;
        private readonly AppDbContext _context;

        public DiscussionController(IDiscussionService service, IHubContext<DiscussionHub> hub, AppDbContext context)
        {
            _service = service;
            _hub = hub;
            _context = context;
        }

        [HttpGet("{inventoryId:guid}")]
        public async Task<IActionResult> Get(Guid inventoryId)
        {
            var result = await _service.GetPostsAsync(inventoryId);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("{inventoryId:guid}")]
        public async Task<IActionResult> Post(Guid inventoryId, [FromBody] string content)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _service.AddPostAsync(userId, inventoryId, content);

            await _hub.Clients
                .Group(inventoryId.ToString())
                .SendAsync("ReceiveMessage", result);

            var inventory = await _context.Inventories
                .Where(i => i.Id == inventoryId)
                .Select(i => new { i.OwnerId, i.Title })
                .FirstAsync();

            if (inventory.OwnerId != userId)
            {
                await _hub.Clients
                    .User(inventory.OwnerId.ToString())
                    .SendAsync("NewNotification", new
                    {
                        inventoryTitle = inventory.Title,
                        message = $"{result.AuthorName} wrote in discussion"
                    });
            }

            return Ok(result);
        }
    }
}
