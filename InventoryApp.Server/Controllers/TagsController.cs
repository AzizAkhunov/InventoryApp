using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TagsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var tags = await _context.Tags
                .Where(t => t.Name.StartsWith(query.ToLower()))
                .OrderBy(t => t.Name)
                .Take(10)
                .Select(t => t.Name)
                .ToListAsync();

            return Ok(tags);
        }

        [HttpGet("cloud")]
        public async Task<IActionResult> GetCloud()
        {
            var cloud = await _context.InventoryTags
                .GroupBy(it => it.Tag.Name)
                .Select(g => new
                {
                    Tag = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(20)
                .ToListAsync();

            return Ok(cloud);
        }
    }
}
