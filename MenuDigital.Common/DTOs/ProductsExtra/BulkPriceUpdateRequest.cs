using System.ComponentModel.DataAnnotations;

namespace MenuDigital.Common.DTOs.ProductsExtras;

public class BulkPriceUpdateRequest
{
    [Range(-90, 500)]
    public decimal Percent { get; set; }
}
