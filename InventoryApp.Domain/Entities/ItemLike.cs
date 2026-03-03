namespace InventoryApp.Domain.Entities
{
    public class ItemLike
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
