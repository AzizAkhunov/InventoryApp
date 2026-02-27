using InventoryApp.Domain.Enums;

namespace InventoryApp.Domain.Entities
{
    public class InventoryIdElement : BaseEntity
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public int Order { get; set; }

        public IdElementType Type { get; set; }

        public string? FixedText { get; set; }

        public int? Padding { get; set; }
    }
}
