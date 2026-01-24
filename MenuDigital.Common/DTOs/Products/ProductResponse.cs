namespace MenuDigital.Common.DTOs.Products;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DiscountPercent { get; set; }
    public bool HappyHourEnabled { get; set; }
    public bool IsFavorite { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal FinalPrice { get; set; }
}
