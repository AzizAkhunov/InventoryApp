using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.DTO
{
    public class StringFieldStatsDto
    {
        public string Field { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int Count { get; set; }
    }
}
