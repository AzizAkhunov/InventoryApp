using InventoryApp.Application.Extensions;
using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _service;

        public LikesController(ILikeService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("{itemId:guid}")]
        public async Task<IActionResult> Toggle(Guid itemId)
        {
            var userId = User.GetUserId();

            var result = await _service.ToggleLikeAsync(userId, itemId);

            return Ok(new
            {
                likesCount = result.likesCount,
                likedByMe = result.likedByMe
            });
        }
    }
}
