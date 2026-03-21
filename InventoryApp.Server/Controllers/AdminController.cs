using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly AppDbContext _context;

        public AdminController(IAdminService adminService, AppDbContext context)
        {
            _adminService = adminService;
            _context = context;
        }

        private async Task<bool> IsCurrentUserAdmin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return false;

            if (!Guid.TryParse(userId, out var guid))
                return false;

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == guid);

            return user != null && user.IsAdmin;
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var users = await _adminService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("block/{userId:guid}")]
        public async Task<IActionResult> Block(Guid userId)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var result = await _adminService.BlockUserAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }        

        [Authorize]
        [HttpPost("unblock/{userId:guid}")]
        public async Task<IActionResult> Unblock(Guid userId)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var result = await _adminService.UnblockUserAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpPost("make-admin/{userId:guid}")]
        public async Task<IActionResult> MakeAdmin(Guid userId)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var result = await _adminService.MakeAdminAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpPost("remove-admin/{userId:guid}")]
        public async Task<IActionResult> RemoveAdmin(Guid userId)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var result = await _adminService.RemoveAdminAsync(userId);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var stats = await _adminService.GetStatsAsync();
            return Ok(stats);
        }

        [Authorize]
        [HttpGet("inventories")]
        public async Task<IActionResult> GetInventories()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var inventories = await _adminService.GetInventoriesAsync();
            return Ok(inventories);
        }

        [Authorize]
        [HttpDelete("inventories/{id:guid}")]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            var result = await _adminService.DeleteInventoryAsync(id);
            if (!result) return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpDelete("users/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("reset-db")]
        public async Task<IActionResult> ResetDb()
        {
            await _context.Database.ExecuteSqlRawAsync("DROP TABLE \"Inventories\" CASCADE;");
            return Ok("Dropped");
        }
    }
}