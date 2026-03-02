using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly AppDbContext _context;

        public DiscussionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DiscussionPostDto>> GetPostsAsync(Guid inventoryId)
        {
            return await _context.DiscussionPosts
                .Where(p => p.InventoryId == inventoryId)
                .OrderBy(p => p.CreatedAt)
                .Select(p => new DiscussionPostDto
                {
                    Id = p.Id,
                    InventoryId = p.InventoryId,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.UserName,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<DiscussionPostDto> AddPostAsync(Guid userId, Guid inventoryId, string content)
        {
            var post = new DiscussionPost
            {
                Id = Guid.NewGuid(),
                InventoryId = inventoryId,
                AuthorId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.DiscussionPosts.Add(post);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstAsync(u => u.Id == userId);

            return new DiscussionPostDto
            {
                Id = post.Id,
                InventoryId = inventoryId,
                AuthorId = userId,
                AuthorName = user.UserName,
                Content = content,
                CreatedAt = post.CreatedAt
            };
        }
    }
}
