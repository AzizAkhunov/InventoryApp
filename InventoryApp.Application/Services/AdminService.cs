using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetUsersAsync()
        {
            return await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.IsAdmin,
                    u.IsBlocked
                })
                .ToListAsync();
        }

        public async Task<bool> BlockUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsBlocked = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnblockUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsBlocked = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MakeAdminAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsAdmin = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAdminAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsAdmin = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<object> GetStatsAsync()
        {
            var users = await _context.Users.CountAsync();
            var inventories = await _context.Inventories.CountAsync();
            var items = await _context.Items.CountAsync();
            var blockedUsers = await _context.Users.CountAsync(x => x.IsBlocked);

            return new
            {
                users,
                inventories,
                items,
                blockedUsers
            };
        }

        public async Task<IEnumerable<object>> GetInventoriesAsync()
        {
            return await _context.Inventories
                .Include(i => i.Owner)
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    Owner = i.Owner.UserName,
                    i.IsPublic,
                    i.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteInventoryAsync(Guid id)
        {
            var inv = await _context.Inventories.FindAsync(id);
            if (inv == null) return false;

            _context.Inventories.Remove(inv);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
