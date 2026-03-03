using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("by-inventory/{inventoryId:guid}")]
        public async Task<ActionResult<List<ItemDto>>> GetByInventory(Guid inventoryId)
        {
            var result = await _itemService.GetByInventoryAsync(inventoryId);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var result = await _itemService.GetByIdAsync(id);   

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromBody] ItemDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var result = await _itemService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (DbUpdateException)
            {
                return Conflict("CustomId must be unique inside inventory.");
            }
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ItemDto>> Update(Guid id, [FromBody] ItemDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var result = await _itemService.UpdateAsync(userId, id, dto);

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
                return Conflict("The item was modified by another user.");
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var result = await _itemService.DeleteAsync(userId, id);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
