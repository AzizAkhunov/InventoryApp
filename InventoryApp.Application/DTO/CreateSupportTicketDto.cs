using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Application.DTO
{
    public class CreateSupportTicketDto
    {
        [Required]
        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(High|Average|Low)$")]
        public string Priority { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Link { get; set; } = string.Empty;

        public Guid? InventoryId { get; set; }
    }
}
