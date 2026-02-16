namespace InventoryApp.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
