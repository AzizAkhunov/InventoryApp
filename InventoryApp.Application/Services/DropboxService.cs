using InventoryApp.Application.Common;
using InventoryApp.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace InventoryApp.Application.Services
{
    public class DropboxService : IDropboxService
    {
        private readonly HttpClient _httpClient;
        private readonly DropboxSettings _settings;

        public DropboxService(HttpClient httpClient, IOptions<DropboxSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> UploadJsonAsync(string fileName, string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(_settings.AccessToken))
                throw new Exception("Dropbox access token is missing.");

            var folder = string.IsNullOrWhiteSpace(_settings.Folder)
                ? "/support-tickets"
                : _settings.Folder.TrimEnd('/');

            var dropboxPath = $"{folder}/{fileName}";

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://content.dropboxapi.com/2/files/upload");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

            request.Headers.Add("Dropbox-API-Arg",
                $"{{\"path\":\"{dropboxPath}\",\"mode\":\"add\",\"autorename\":true,\"mute\":false}}");

            request.Headers.Add("Content-Type", "application/octet-stream");

            request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonContent));
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Dropbox upload error: " + responseBody);

            return dropboxPath;
        }
    }
}
