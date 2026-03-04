using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.DTO
{
    public class NumberFieldStatsDto
    {
        public string Field { get; set; } = null!;
        public double? Avg { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
    }
}
