using MenuDigital.Common.DTOs.Products;
using MenuDigital.Common.DTOs.ProductsExtras;

namespace MenuDigital.Services.Products;

public interface IProductService
{
    Task<List<ProductResponse>> GetMineAsync(int restaurantUserId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted);
    Task<ProductResponse> CreateAsync(int restaurantUserId, ProductCreateRequest request);
    Task<ProductResponse> UpdateAsync(int restaurantUserId, int productId, ProductUpdateRequest request);
    Task<int> BulkUpdatePricesAsync(int restaurantUserId, BulkPriceUpdateRequest request);

    Task DeleteAsync(int restaurantUserId, int productId);
}
