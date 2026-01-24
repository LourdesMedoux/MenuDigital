namespace MenuDigital.Common.DTOs.Account;

public class AccountResponse
{
    public int Id { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
