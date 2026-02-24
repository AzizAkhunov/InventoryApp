namespace InventoryApp.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
