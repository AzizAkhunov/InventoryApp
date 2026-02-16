namespace InventoryApp.Domain.Entities
{
    public class InventoryTag
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public Guid TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
