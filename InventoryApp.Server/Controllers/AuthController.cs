using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwt;

        public AuthController(AppDbContext context, IJwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("login/{userId:guid}")]
        public async Task<IActionResult> Login(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var token = _jwt.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
