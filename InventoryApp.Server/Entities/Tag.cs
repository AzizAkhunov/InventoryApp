namespace InventoryApp.Server.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<InventoryTag> InventoryTags { get; set; } = new List<InventoryTag>();
    }
}
