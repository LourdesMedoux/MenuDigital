using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.Entities;

public class RestaurantUser
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string RestaurantName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string OwnerName { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // Navegación
    public List<Category> Categories { get; set; } = new();
    public List<Product> Products { get; set; } = new();
}
