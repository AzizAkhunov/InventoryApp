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
                .AsNoTracking()
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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;

            user.IsBlocked = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;

            user.IsBlocked = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MakeAdminAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;

            user.IsAdmin = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAdminAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;

            user.IsAdmin = false;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetStatsAsync()
        {
            var usersTask = _context.Users.CountAsync();
            var inventoriesTask = _context.Inventories.CountAsync();
            var itemsTask = _context.Items.CountAsync();
            var blockedUsersTask = _context.Users.CountAsync(x => x.IsBlocked);

            await Task.WhenAll(usersTask, inventoriesTask, itemsTask, blockedUsersTask);

            return new
            {
                users = usersTask.Result,
                inventories = inventoriesTask.Result,
                items = itemsTask.Result,
                blockedUsers = blockedUsersTask.Result
            };
        }

        public async Task<IEnumerable<object>> GetInventoriesAsync()
        {
            return await _context.Inventories
                .AsNoTracking()
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
            var inv = await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            if (inv == null) return false;

            _context.Inventories.Remove(inv);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}