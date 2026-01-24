using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.Entities;

public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    // FK
    public int RestaurantUserId { get; set; }
    public RestaurantUser? RestaurantUser { get; set; }

    // Navegación
    public List<Product> Products { get; set; } = new();
}
