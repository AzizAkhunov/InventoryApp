using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid InventoryId { get; set; }   
        public string InventoryTitle { get; set; } = null!;

        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }

    }
}
