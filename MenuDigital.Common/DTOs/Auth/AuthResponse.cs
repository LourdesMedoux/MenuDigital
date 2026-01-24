namespace MenuDigital.Common.DTOs.Auth;

public class AuthResponse
{
    public int RestaurantUserId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
