using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly IDiscussionService _service;

        public DiscussionController(IDiscussionService service)
        {
            _service = service;
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
            return Ok(result);
        }
    }
}
