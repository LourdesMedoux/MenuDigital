using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.Products;

public class ProductUpdateRequest
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    [Range(0, 100)]
    public int DiscountPercent { get; set; } = 0;

    public bool HappyHourEnabled { get; set; } = false;

    public bool IsFavorite { get; set; } = false;
}
