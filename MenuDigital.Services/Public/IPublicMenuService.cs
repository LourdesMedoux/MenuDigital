using MenuDigital.Common.DTOs.Public;

namespace MenuDigital.Services.Public;

public interface IPublicMenuService
{
    Task<List<PublicRestaurantResponse>> GetRestaurantsAsync();
    Task<List<PublicProductResponse>> GetMenuAsync(int restaurantId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted);
}
