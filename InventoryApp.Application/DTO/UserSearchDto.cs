using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.DTO
{
    public class UserSearchDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
    }
}
