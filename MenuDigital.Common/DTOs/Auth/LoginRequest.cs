using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.Auth;

public class LoginRequest
{
    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
