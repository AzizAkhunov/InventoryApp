using InventoryApp.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.Interfaces
{
    public interface IInventoryExternalService
    {
        Task<string> GenerateTokenAsync(Guid userId, Guid inventoryId);
        Task<InventoryExternalDto?> GetByTokenAsync(string token);
    }
}
