using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }

        public ICollection<Inventory> OwnedInventories { get; set; } = new List<Inventory>();
        public ICollection<InventoryAccess> AccessibleInventories { get; set; } = new List<InventoryAccess>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
