using System.Security.Claims;

namespace MenuDigital.Api.Security;

public static class ClaimsExtensions
{
    public static int GetRestaurantUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue("restaurantUserId");
        if (string.IsNullOrWhiteSpace(value)) throw new UnauthorizedAccessException("Token inválido.");
        if (!int.TryParse(value, out var id)) throw new UnauthorizedAccessException("Token inválido.");
        return id;
    }
}
