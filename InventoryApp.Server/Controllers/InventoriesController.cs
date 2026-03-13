using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly AppDbContext _context;
        public InventoriesController(IInventoryService inventoryService,AppDbContext context)
        {
            _inventoryService = inventoryService;
            _context = context;
        }



        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyInventories()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var inventories = await _context.Inventories
                .Where(i => i.OwnerId == userId)
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    i.Description,
                    i.ImageUrl,
                    i.IsPublic
                })
                .ToListAsync();

            return Ok(inventories);
        }

        [Authorize]
        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedInventories()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var inventories = await _context.InventoryAccesses
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.Inventory.Id,
                    a.Inventory.Title,
                    a.Inventory.Description,
                    a.Inventory.ImageUrl,
                    a.Inventory.IsPublic
                })
                .ToListAsync();

            return Ok(inventories);
        }


        [HttpGet]
        public async Task<ActionResult<List<InventoryDto>>> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<InventoryDto>> GetById(Guid id)
        {
            var result = await _inventoryService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InventoryDto>> Create([FromBody] InventoryDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _inventoryService.CreateAsync(userId, dto);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<InventoryDto>> Update(Guid id, [FromBody] InventoryDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var result = await _inventoryService.UpdateAsync(userId, id, dto);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The record was modified by another user.");
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var result = await _inventoryService.DeleteAsync(userId, id);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id:guid}/statistics")]
        public async Task<ActionResult<InventoryStatisticsDto>> GetStatistics(Guid id)
        {
            var result = await _inventoryService.GetStatisticsAsync(id);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{inventoryId:guid}/custom-id")]
        public async Task<IActionResult> SaveCustomId(Guid inventoryId, [FromBody] List<InventoryIdElementDto> elements)
        {
            var existing = _context.InventoryIdElements
                .Where(e => e.InventoryId == inventoryId);

            _context.InventoryIdElements.RemoveRange(existing);

            var order = 0;

            foreach (var el in elements)
            {
                _context.InventoryIdElements.Add(new InventoryIdElement
                {
                    InventoryId = inventoryId,
                    Order = order++,
                    Type = el.Type,
                    FixedText = el.FixedText,
                    Padding = el.Padding
                });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("by-tag")]
        public async Task<IActionResult> GetByTag([FromQuery] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return Ok(new List<InventoryDto>());

            var inventories = await _context.InventoryTags
                .Where(it => it.Tag.Name.ToLower() == tag.ToLower())
                .Select(it => it.Inventory)
                .Select(i => new InventoryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                    IsPublic = i.IsPublic
                })
                .ToListAsync();

            return Ok(inventories);
        }
    }
}
