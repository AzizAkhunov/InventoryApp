using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost("{userId:guid}")]
        public async Task<IActionResult> AddAccess(Guid inventoryId, Guid userId)
        {
            var ownerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

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

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> RemoveAccess(Guid inventoryId, Guid userId)
        {
            var ownerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

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
