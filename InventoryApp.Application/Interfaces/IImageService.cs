using Microsoft.AspNetCore.Http;

namespace InventoryApp.Application.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
