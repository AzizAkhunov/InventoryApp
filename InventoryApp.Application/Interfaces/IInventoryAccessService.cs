using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface IInventoryAccessService
    {
        Task<List<InventoryAccessDto>> GetAccessListAsync(Guid inventoryId);

        Task AddAccessAsync(Guid ownerId, Guid inventoryId, Guid userId);

        Task RemoveAccessAsync(Guid ownerId, Guid inventoryId, Guid userId);
    }
}
