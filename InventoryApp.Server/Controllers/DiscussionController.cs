using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using InventoryApp.Server.Hubs;


namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly IHubContext<DiscussionHub> _hub;
        private readonly IDiscussionService _service;

        public DiscussionController(IDiscussionService service, IHubContext<DiscussionHub> hub)
        {
            _service = service;
            _hub = hub;
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

            return Ok(result);
        }
    }
}
