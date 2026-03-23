using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.Interfaces
{
    public interface IDropboxService
    {
        Task<string> UploadJsonAsync(string fileName, string jsonContent);
    }
}
