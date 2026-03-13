using InventoryApp.Application.Extensions;
using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryAccessController : ControllerBase
    {
        private readonly IInventoryAccessService _service;

        public InventoryAccessController(IInventoryAccessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessList(Guid inventoryId)
        {
            var result = await _service.GetAccessListAsync(inventoryId);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("{userId:guid}")]
        public async Task<IActionResult> AddAccess(Guid inventoryId, Guid userId)
        {
            var ownerId = User.GetUserId();

            try
            {
                await _service.AddAccessAsync(ownerId, inventoryId, userId);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [Authorize]
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> RemoveAccess(Guid inventoryId, Guid userId)
        {
            var ownerId = User.GetUserId();

            try
            {
                await _service.RemoveAccessAsync(ownerId, inventoryId, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
