using InventoryApp.Application.DTO;
using InventoryApp.Application.Extensions;
using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ISalesforceService _salesforce;
        public UsersController(AppDbContext context, ISalesforceService salesforce)
        {
            _context = context;
            _salesforce = salesforce;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<UserSearchDto>());

            var currentUserId = User.GetUserId();

            var users = await _context.Users
                .Where(u => u.Id != currentUserId)
                .Where(u => u.Email.ToLower().Contains(query.ToLower()))
                .OrderBy(u => u.Email)
                .Select(u => new UserSearchDto
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .Take(10)
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("salesforce")]
        public async Task<IActionResult> CreateSalesforce([FromBody] SalesforceDto dto)
        {
            await _salesforce.CreateAccountAsync(dto);
            return Ok(new { success = true });
        }
    }
}
