using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface ITagService
    {
        Task<List<string>> SearchAsync(string query);

        Task<List<string>> AutocompleteAsync(string query);

        Task<List<TagCloudDto>> GetCloudAsync();
    }
}
