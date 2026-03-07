using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetByInventoryAsync(Guid inventoryId);

        Task<ItemDto?> GetByIdAsync(Guid id);

        Task<ItemDto> CreateAsync(Guid userId, CreateItemDto dto);

        Task<ItemDto?> UpdateAsync(Guid userId, Guid id, ItemDto dto);

        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
