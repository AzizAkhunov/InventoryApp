namespace InventoryApp.Application.DTO
{
    public class SupportTicketFileDto
    {
        public string ReportedBy { get; set; } = string.Empty;
        public string? Inventory { get; set; }
        public string Link { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public List<string> AdminEmails { get; set; } = new();
        public string CreatedAtUtc { get; set; } = string.Empty;
    }
}
