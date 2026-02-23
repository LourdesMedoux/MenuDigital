using MenuDigital.Common.DTOs.Public;
using MenuDigital.Data.Repositories.Interfaces;
using System.Linq;

namespace MenuDigital.Services.Public;

public class PublicMenuService : IPublicMenuService
{
    private readonly IRestaurantUserRepository _restaurantRepository;
    private readonly IProductRepository _productRepository;

    public PublicMenuService(IRestaurantUserRepository restaurantRepository, IProductRepository productRepository)
    {
        _restaurantRepository = restaurantRepository;
        _productRepository = productRepository;
    }

    public async Task<List<PublicRestaurantResponse>> GetRestaurantsAsync()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();

        return restaurants
            .OrderBy(x => x.RestaurantName)
            .Select(x => new PublicRestaurantResponse
            {
                Id = x.Id,
                RestaurantName = x.RestaurantName
            })
            .ToList();
    }

    public async Task<List<PublicProductResponse>> GetMenuAsync(int restaurantId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted)
    {
        var products = await _productRepository.GetByRestaurantAsync(restaurantId);
        var query = products.AsQueryable();

        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (onlyFavorites == true) query = query.Where(x => x.IsFavorite);
        if (onlyDiscounted == true) query = query.Where(x => x.HappyHourEnabled && x.DiscountPercent > 0);

        return query
            .OrderBy(x => x.Name)
            .Select(x => new PublicProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                DiscountPercent = x.DiscountPercent,
                HappyHourEnabled = x.HappyHourEnabled,
                IsFavorite = x.IsFavorite,
                CategoryName = x.Category != null ? x.Category.Name : null,
                FinalPrice = x.HappyHourEnabled && x.DiscountPercent > 0
                    ? x.Price * (1 - (x.DiscountPercent / 100m))
                    : x.Price
            })
            .ToList();
    }
}
