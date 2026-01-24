using MenuDigital.Common.DTOs.Public;
using MenuDigital.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Public;

public class PublicMenuService : IPublicMenuService
{
    private readonly AppDbContext _db;

    public PublicMenuService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<PublicRestaurantResponse>> GetRestaurantsAsync()
    {
        return await _db.RestaurantUsers
            .AsNoTracking()
            .OrderBy(x => x.RestaurantName)
            .Select(x => new PublicRestaurantResponse
            {
                Id = x.Id,
                RestaurantName = x.RestaurantName
            })
            .ToListAsync();
    }

    public async Task<List<PublicProductResponse>> GetMenuAsync(int restaurantId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted)
    {
        var query = _db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.RestaurantUserId == restaurantId);

        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (onlyFavorites == true) query = query.Where(x => x.IsFavorite);
        if (onlyDiscounted == true) query = query.Where(x => x.HappyHourEnabled && x.DiscountPercent > 0);

        return await query
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
            .ToListAsync();
    }
}
