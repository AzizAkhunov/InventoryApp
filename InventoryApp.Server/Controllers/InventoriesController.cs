using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
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
    }
}
