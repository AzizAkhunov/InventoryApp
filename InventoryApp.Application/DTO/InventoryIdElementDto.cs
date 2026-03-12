using InventoryApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.DTO
{
    public class InventoryIdElementDto
    {
        public IdElementType Type { get; set; }

        public string? FixedText { get; set; }

        public int? Padding { get; set; }
    }
}
