namespace InventoryApp.Server.Entities
{
    public class InventoryField : BaseEntity
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public string FieldType { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int SlotNumber { get; set; }
        public bool ShowInTable { get; set; }
        public int Order { get; set; }
    }
}
