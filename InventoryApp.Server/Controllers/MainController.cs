using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MainController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var latest = await _context.Inventories
                .OrderByDescending(i => i.CreatedAt)
                .Take(10)
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    i.Description,
                    i.ImageUrl
                })
                .ToListAsync();

            return Ok(latest);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var popular = await _context.Inventories
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    ItemsCount = _context.Items.Count(it => it.InventoryId == i.Id)
                })
                .OrderByDescending(x => x.ItemsCount)
                .Take(5)
                .ToListAsync();

            return Ok(popular);
        }
    }
}
