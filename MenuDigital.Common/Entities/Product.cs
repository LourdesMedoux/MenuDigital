using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.Entities;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    // Descuento
    [Range(0, 100)]
    public int DiscountPercent { get; set; } = 0;

    // Happy hour (si está activo, se aplica el descuento)
    public bool HappyHourEnabled { get; set; } = false;

    // Favorito (si el TP lo pide como “destacado” del menú)
    public bool IsFavorite { get; set; } = false;

    // FK Restaurant
    public int RestaurantUserId { get; set; }
    public RestaurantUser? RestaurantUser { get; set; }

    // FK Category (opcional, por si querés productos “sin categoría”)
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
