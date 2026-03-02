namespace InventoryApp.Application.DTO
{
    public class DiscussionPostDto
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
