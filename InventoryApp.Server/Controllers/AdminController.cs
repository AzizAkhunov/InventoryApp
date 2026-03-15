using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("block/{userId:guid}")]
        public async Task<IActionResult> Block(Guid userId)
        {
            var result = await _adminService.BlockUserAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpPost("unblock/{userId:guid}")]
        public async Task<IActionResult> Unblock(Guid userId)
        {
            var result = await _adminService.UnblockUserAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpPost("make-admin/{userId:guid}")]
        public async Task<IActionResult> MakeAdmin(Guid userId)
        {
            var result = await _adminService.MakeAdminAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpPost("remove-admin/{userId:guid}")]
        public async Task<IActionResult> RemoveAdmin(Guid userId)
        {
            var result = await _adminService.RemoveAdminAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetStatsAsync();
            return Ok(stats);
        }

        [Authorize]
        [HttpGet("inventories")]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _adminService.GetInventoriesAsync();
            return Ok(inventories);
        }

        [Authorize]
        [HttpDelete("inventories/{id:guid}")]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            var result = await _adminService.DeleteInventoryAsync(id);
            if (!result) return NotFound();

            return Ok();
        }

    }
}
