using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var tags = await _tagService.SearchAsync(query);

            return Ok(tags);
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] string query)
        {
            var tags = await _tagService.AutocompleteAsync(query);

            return Ok(tags);
        }

        [HttpGet("cloud")]
        public async Task<IActionResult> GetCloud()
        {
            var cloud = await _tagService.GetCloudAsync();

            return Ok(cloud);
        }
    }
}
