using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<object>> GetUsersAsync();

        Task<bool> BlockUserAsync(Guid userId);
        Task<bool> UnblockUserAsync(Guid userId);

        Task<bool> MakeAdminAsync(Guid userId);
        Task<bool> RemoveAdminAsync(Guid userId);

        Task<object> GetStatsAsync();

        Task<IEnumerable<object>> GetInventoriesAsync();

        Task<bool> DeleteInventoryAsync(Guid id);
    }
}
