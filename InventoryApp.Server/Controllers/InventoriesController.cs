using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoriesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
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

        [HttpPost]
        public async Task<ActionResult<InventoryDto>> Create([FromBody] InventoryDto dto)
        {
            // временно userId захардкожен
            var userId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");

            var result = await _inventoryService.CreateAsync(userId, dto);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<InventoryDto>> Update(Guid id, [FromBody] InventoryDto dto)
        {
            var userId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");

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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");

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
    }
}
