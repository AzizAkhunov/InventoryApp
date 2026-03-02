namespace InventoryApp.Application.DTO
{
    public class SearchResultDto
    {
        public string Type { get; set; } = null!;
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? CustomId { get; set; }
    }
}
