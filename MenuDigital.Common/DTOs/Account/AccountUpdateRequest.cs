using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.Account;

public class AccountUpdateRequest
{
    [Required, MaxLength(120)]
    public string RestaurantName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string OwnerName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string CurrentPassword { get; set; } = string.Empty;

    [MinLength(6), MaxLength(100)]
    public string? NewPassword { get; set; }
}
