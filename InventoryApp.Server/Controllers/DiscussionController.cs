using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost("{inventoryId:guid}")]
        public async Task<IActionResult> Post(Guid inventoryId, [FromBody] string content)
        {
            var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var result = await _service.AddPostAsync(userId, inventoryId, content);
            return Ok(result);
        }
    }
}
