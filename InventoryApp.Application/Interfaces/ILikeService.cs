namespace InventoryApp.Application.Interfaces
{
    public interface ILikeService
    {
        Task<int> ToggleLikeAsync(Guid userId, Guid itemId);
    }
}
