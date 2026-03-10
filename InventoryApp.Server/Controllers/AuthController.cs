using Google.Apis.Auth;
using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryApp.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthController(
            AppDbContext context,
            IJwtService jwtService,
            IConfiguration config)
        {
            _context = context;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.IdToken))
                return BadRequest("IdToken is required.");

            GoogleJsonWebSignature.Payload payload;

            try
            {
                var clientId = _config["GoogleAuth:ClientId"];

                payload = await GoogleJsonWebSignature.ValidateAsync(
                    dto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { clientId }
                    });
            }
            catch
            {
                return Unauthorized("Invalid Google token.");
            }

            var email = payload.Email;
            var googleName = payload.Name;
            var picture = payload.Picture;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    UserName = googleName,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            if (user.IsBlocked)
                return Forbid();

            var jwt = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token = jwt,
                user = new
                {
                    id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    picture = picture,
                    isAdmin = user.IsAdmin,
                    isBlocked = user.IsBlocked,
                    createdAt = user.CreatedAt
                }
            });
        }


        [Authorize]
        [HttpPut("username")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            user.UserName = dto.UserName;

            await _context.SaveChangesAsync();

            var jwt = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token = jwt,
                user = new
                {
                    id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    isAdmin = user.IsAdmin,
                    isBlocked = user.IsBlocked,
                    createdAt = user.CreatedAt
                }
            });
        }
    }
}