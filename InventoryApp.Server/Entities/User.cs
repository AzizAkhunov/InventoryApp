using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Server.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }

        public ICollection<Inventory> OwnedInventories { get; set; } = new List<Inventory>();
    }
}
