using InventoryApp.Application.Extensions;
using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/external/inventories")]
    [ApiController]
    public class ExternalInventoriesController : ControllerBase
    {
        private readonly IInventoryExternalService _service;

        public ExternalInventoriesController(IInventoryExternalService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("{inventoryId:guid}/token")]
        public async Task<IActionResult> GenerateToken(Guid inventoryId)
        {
            var userId = User.GetUserId();

            try
            {
                var token = await _service.GenerateTokenAsync(userId, inventoryId);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }

        [HttpGet("import")]
        public async Task<IActionResult> ImportByToken([FromQuery] string token)
        {
            var result = await _service.GetByTokenAsync(token);

            if (result == null)
                return NotFound(new { message = "Invalid token" });

            return Ok(result);
        }
    }
}