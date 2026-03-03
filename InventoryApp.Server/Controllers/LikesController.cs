using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _service;

        public LikesController(ILikeService service)
        {
            _service = service;
        }

        [HttpPost("{itemId:guid}")]
        public async Task<IActionResult> Toggle(Guid itemId)
        {
            var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var count = await _service.ToggleLikeAsync(userId, itemId);

            return Ok(new { Likes = count });
        }
    }
}
