using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface IDiscussionService
    {
        Task<List<DiscussionPostDto>> GetPostsAsync(Guid inventoryId);
        Task<DiscussionPostDto> AddPostAsync(Guid userId, Guid inventoryId, string content);
    }
}
