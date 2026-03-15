using System.Security.Claims;

namespace InventoryApp.Application.Extensions
{
    public static class GetUserExtencion
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
}
