using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int likesCount, bool likedByMe)> ToggleLikeAsync(Guid userId, Guid itemId)
        {
            var existing = await _context.ItemLikes
                .FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

            bool likedByMe;

            if (existing != null)
            {
                _context.ItemLikes.Remove(existing);
                likedByMe = false;
            }
            else
            {
                _context.ItemLikes.Add(new ItemLike
                {
                    ItemId = itemId,
                    UserId = userId
                });

                likedByMe = true;
            }

            await _context.SaveChangesAsync();

            var count = await _context.ItemLikes
                .CountAsync(l => l.ItemId == itemId);

            return (count, likedByMe);
        }
    }
}
