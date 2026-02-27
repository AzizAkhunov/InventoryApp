namespace InventoryApp.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsPublic { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public int Version { get; set; }

        // ===== Custom STRING fields =====
        public bool CustomString1Enabled { get; set; }
        public string? CustomString1Name { get; set; }

        public bool CustomString2Enabled { get; set; }
        public string? CustomString2Name { get; set; }

        public bool CustomString3Enabled { get; set; }
        public string? CustomString3Name { get; set; }

        // ===== Custom NUMBER fields =====
        public bool CustomNumber1Enabled { get; set; }
        public string? CustomNumber1Name { get; set; }

        public bool CustomNumber2Enabled { get; set; }
        public string? CustomNumber2Name { get; set; }

        public bool CustomNumber3Enabled { get; set; }
        public string? CustomNumber3Name { get; set; }

        // ===== Custom BOOLEAN fields =====
        public bool CustomBool1Enabled { get; set; }
        public string? CustomBool1Name { get; set; }

        public bool CustomBool2Enabled { get; set; }
        public string? CustomBool2Name { get; set; }

        public bool CustomBool3Enabled { get; set; }
        public string? CustomBool3Name { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<InventoryAccess> AccessList { get; set; } = new List<InventoryAccess>();
        public ICollection<InventoryTag> InventoryTags { get; set; } = new List<InventoryTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<InventoryIdElement> IdElements { get; set; } = new List<InventoryIdElement>();
    }
}
