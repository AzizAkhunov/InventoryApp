namespace InventoryApp.Application.DTO
{
    public class InventoryAccessDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
