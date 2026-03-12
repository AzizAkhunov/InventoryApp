namespace InventoryApp.Application.Interfaces
{
    public interface ILikeService
    {
        Task<(int likesCount, bool likedByMe)> ToggleLikeAsync(Guid userId, Guid itemId);
    }
}
