using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var inventories = await _context.Inventories.CountAsync();
            var items = await _context.Items.CountAsync();
            var posts = await _context.DiscussionPosts.CountAsync();
            var users = await _context.Users.CountAsync();

            return Ok(new
            {
                inventories,
                items,
                posts,
                users
            });
        }

        [HttpGet("activity")]
        public IActionResult GetActivity()
        {
            var data = new[]
            {
            new { month = "Jan", value = 12 },
            new { month = "Feb", value = 18 },
            new { month = "Mar", value = 26 },
            new { month = "Apr", value = 35 },
            new { month = "May", value = 48 },
            new { month = "Jun", value = 52 }
        };

            return Ok(data);
        }
    }
}
