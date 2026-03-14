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
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.IsAdmin,
                    u.IsBlocked
                })
                .ToListAsync();

            return Ok(users);
        }
        [Authorize]
        [HttpPost("block/{userId:guid}")]
        public async Task<IActionResult> Block(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = true;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Authorize]
        [HttpPost("unblock/{userId:guid}")]
        public async Task<IActionResult> Unblock(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("make-admin/{userId:guid}")]
        public async Task<IActionResult> MakeAdmin(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsAdmin = true;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Authorize]
        [HttpPost("remove-admin/{userId:guid}")]
        public async Task<IActionResult> RemoveAdmin(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsAdmin = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var users = await _context.Users.CountAsync();
            var inventories = await _context.Inventories.CountAsync();
            var items = await _context.Items.CountAsync();
            var blockedUsers = await _context.Users.CountAsync(x => x.IsBlocked);

            return Ok(new
            {
                users,
                inventories,
                items,
                blockedUsers
            });
        }

        [Authorize]
        [HttpGet("inventories")]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _context.Inventories
                .Include(i => i.Owner)
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    Owner = i.Owner.UserName,
                    i.IsPublic,
                    i.CreatedAt
                })
                .ToListAsync();

            return Ok(inventories);
        }

        [Authorize]
        [HttpDelete("inventories/{id:guid}")]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            var inv = await _context.Inventories.FindAsync(id);
            if (inv == null) return NotFound();

            _context.Inventories.Remove(inv);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
