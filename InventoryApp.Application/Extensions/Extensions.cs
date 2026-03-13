using System.Security.Claims;

namespace InventoryApp.Application.Extensions
{
    public static class Extensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}
