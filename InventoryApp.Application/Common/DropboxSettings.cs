using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.Common
{
    public class DropboxSettings
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Folder { get; set; } = "/support-tickets";
    }
}
