using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.Account;

public class AccountDeleteRequest
{
    [Required, MinLength(6), MaxLength(100)]
    public string CurrentPassword { get; set; } = string.Empty;
}
