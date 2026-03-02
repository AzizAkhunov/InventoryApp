using InventoryApp.Application.DTO;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<object>());

            var normalized = query.ToLower();

            var inventories = await _context.Inventories
                .Where(i =>
                    EF.Functions.ToTsVector("simple",
                    i.Title + " " + i.Description)
                .Matches(EF.Functions.PlainToTsQuery("simple", normalized)))
                .Select(i => new SearchResultDto
                {
                    Type = "Inventory",
                    Id = i.Id,
                    Title = i.Title,
                    CustomId = null
                })
                .ToListAsync();

            var items = await _context.Items
                .Where(i =>
                    EF.Functions.ToTsVector("simple",
                    i.Text1 + " " + i.MultiText1)
                .Matches(EF.Functions.PlainToTsQuery("simple", normalized)))
                .Select(i => new SearchResultDto
                {
                    Type = "Item",
                    Id = i.Id,
                    Title = null,
                    CustomId = i.CustomId
                })
                .ToListAsync();

            return Ok(inventories.Concat(items));
        }
    }
}
