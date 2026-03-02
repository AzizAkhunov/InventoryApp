using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Domain.Entities
{
    public class DiscussionPost : BaseEntity
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public User Author { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
