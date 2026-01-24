namespace MenuDigital.Common.DTOs.Public;

public class PublicProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal FinalPrice { get; set; }
    public bool IsFavorite { get; set; }
    public bool HappyHourEnabled { get; set; }
    public int DiscountPercent { get; set; }
    public string? CategoryName { get; set; }
}
