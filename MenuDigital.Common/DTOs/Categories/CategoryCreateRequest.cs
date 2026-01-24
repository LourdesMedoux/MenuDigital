using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.Categories;

public class CategoryCreateRequest
{
    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;
}
