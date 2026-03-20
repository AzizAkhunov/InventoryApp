using InventoryApp.Application.DTO;

namespace InventoryApp.Application.Interfaces
{
    public interface ISalesforceService
    {
        Task CreateAccountAsync(SalesforceDto dto);
    }
}
