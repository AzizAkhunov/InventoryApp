using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost("block/{userId:guid}")]
        public async Task<IActionResult> Block(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("unblock/{userId:guid}")]
        public async Task<IActionResult> Unblock(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("make-admin/{userId:guid}")]
        public async Task<IActionResult> MakeAdmin(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsAdmin = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("remove-admin/{userId:guid}")]
        public async Task<IActionResult> RemoveAdmin(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsAdmin = false;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
