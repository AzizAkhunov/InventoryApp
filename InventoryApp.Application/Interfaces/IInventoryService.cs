using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<List<InventoryDto>> GetAllAsync();

        Task<InventoryDto?> GetByIdAsync(Guid id);

        Task<InventoryDto> CreateAsync(Guid userId, InventoryDto dto);

        Task<InventoryDto?> UpdateAsync(Guid userId, Guid id, InventoryDto dto);

        Task<bool> DeleteAsync(Guid userId, Guid id);
        Task<InventoryStatisticsDto> GetStatisticsAsync(Guid inventoryId);
    }
}
