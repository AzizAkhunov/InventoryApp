namespace InventoryApp.Server.Entities
{
    public class Like : BaseEntity
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
