namespace InventoryApp.Application.Interfaces
{
    public interface ICustomIdGenerator
    {
        Task<string> GenerateAsync(Guid inventoryId);
    }
}
