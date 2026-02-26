using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class InventoryAccessService : IInventoryAccessService
    {
        private readonly AppDbContext _context;

        public InventoryAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryAccessDto>> GetAccessListAsync(Guid inventoryId)
        {
            return await _context.InventoryAccesses
                .Where(x => x.InventoryId == inventoryId)
                .Include(x => x.User)
                .Select(x => new InventoryAccessDto
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    Email = x.User.Email
                })
                .ToListAsync();
        }

        public async Task AddAccessAsync(Guid ownerId, Guid inventoryId, Guid userId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            if (inventory.OwnerId != ownerId)
                throw new UnauthorizedAccessException();

            var exists = await _context.InventoryAccesses
                .AnyAsync(x => x.InventoryId == inventoryId && x.UserId == userId);

            if (exists)
                return;

            var access = new InventoryAccess
            {
                InventoryId = inventoryId,
                UserId = userId
            };

            _context.InventoryAccesses.Add(access);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAccessAsync(Guid ownerId, Guid inventoryId, Guid userId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            if (inventory.OwnerId != ownerId)
                throw new UnauthorizedAccessException();

            var access = await _context.InventoryAccesses
                .FirstOrDefaultAsync(x => x.InventoryId == inventoryId && x.UserId == userId);

            if (access == null)
                return;

            _context.InventoryAccesses.Remove(access);
            await _context.SaveChangesAsync();
        }
    }
}
