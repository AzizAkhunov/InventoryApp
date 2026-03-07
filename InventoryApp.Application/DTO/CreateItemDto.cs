using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Application.DTO
{
    public class CreateItemDto
    {
        public Guid InventoryId { get; set; }

        public int Version { get; set; }

        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }

        public string? MultiText1 { get; set; }
        public string? MultiText2 { get; set; }
        public string? MultiText3 { get; set; }

        [Range(0,int.MaxValue)]
        public decimal? Number1 { get; set; }
        public decimal? Number2 { get; set; }
        public decimal? Number3 { get; set; }

        public bool? Bool1 { get; set; }
        public bool? Bool2 { get; set; }
        public bool? Bool3 { get; set; }

        public string? Doc1 { get; set; }
        public string? Doc2 { get; set; }
        public string? Doc3 { get; set; }
    }
}
