using InventoryApp.Application.DTO;
using InventoryApp.Application.Extensions;
using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDropboxService _dropboxService;

        public SupportTicketsController(AppDbContext context, IDropboxService dropboxService)
        {
            _context = context;
            _dropboxService = dropboxService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupportTicketDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUserId();

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return Unauthorized(new { message = "User not found." });

            string? inventoryTitle = null;

            if (dto.InventoryId.HasValue)
            {
                var inventory = await _context.Inventories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == dto.InventoryId.Value);

                if (inventory != null)
                    inventoryTitle = inventory.Title;
            }

            var adminEmails = await _context.Users
                .AsNoTracking()
                .Where(x => x.IsAdmin)
                .Select(x => x.Email)
                .Distinct()
                .ToListAsync();

            if (adminEmails.Count == 0)
            {
                return BadRequest(new
                {
                    message = "No admin email addresses configured."
                });
            }

            var ticket = new SupportTicketFileDto
            {
                ReportedBy = user.Email,
                Inventory = inventoryTitle,
                Link = dto.Link.Trim(),
                Summary = dto.Summary.Trim(),
                Priority = dto.Priority.Trim(),
                AdminEmails = adminEmails,
                CreatedAtUtc = DateTime.UtcNow.ToString("O")
            };

            var json = JsonSerializer.Serialize(ticket, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var safeEmail = user.Email.Replace("@", "_at_").Replace(".", "_");
            var fileName = $"support-ticket-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{safeEmail}.json";

            try
            {
                var uploadedPath = await _dropboxService.UploadJsonAsync(fileName, json);

                return Ok(new
                {
                    success = true,
                    fileName,
                    path = uploadedPath
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to upload support ticket to Dropbox.",
                    details = ex.Message
                });
            }
        }
    }
}