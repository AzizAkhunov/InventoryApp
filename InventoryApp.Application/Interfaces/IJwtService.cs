using InventoryApp.Domain.Entities;

namespace InventoryApp.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);    
    }
}
