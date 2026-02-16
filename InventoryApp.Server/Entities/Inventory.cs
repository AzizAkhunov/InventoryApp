namespace InventoryApp.Server.Entities
{
    public class Inventory : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Category { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsPublic { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public int Version { get; set; }
    }
}
