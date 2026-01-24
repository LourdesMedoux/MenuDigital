using MenuDigital.Common.DTOs.Products;
using MenuDigital.Common.DTOs.ProductsExtras;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Products;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductResponse>> GetMineAsync(int restaurantUserId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted)
    {
        var query = _db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.RestaurantUserId == restaurantUserId);

        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (onlyFavorites == true) query = query.Where(x => x.IsFavorite);
        if (onlyDiscounted == true) query = query.Where(x => x.HappyHourEnabled && x.DiscountPercent > 0);

        return await query
            .OrderBy(x => x.Name)
            .Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                DiscountPercent = x.DiscountPercent,
                HappyHourEnabled = x.HappyHourEnabled,
                IsFavorite = x.IsFavorite,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : null,
                FinalPrice = x.HappyHourEnabled && x.DiscountPercent > 0
                    ? x.Price * (1 - (x.DiscountPercent / 100m))
                    : x.Price
            })
            .ToListAsync();
    }

    public async Task<ProductResponse> CreateAsync(int restaurantUserId, ProductCreateRequest request)
    {
        int? categoryId = null;

        if (request.CategoryId.HasValue)
        {
            var catExists = await _db.Categories.AnyAsync(x => x.Id == request.CategoryId.Value && x.RestaurantUserId == restaurantUserId);
            if (!catExists) throw new InvalidOperationException("La categoría no existe o no pertenece a tu restaurante.");
            categoryId = request.CategoryId.Value;
        }

        var entity = new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            CategoryId = categoryId,
            RestaurantUserId = restaurantUserId,
            DiscountPercent = request.DiscountPercent,
            HappyHourEnabled = request.HappyHourEnabled,
            IsFavorite = request.IsFavorite
        };

        _db.Products.Add(entity);
        await _db.SaveChangesAsync();

        var categoryName = categoryId.HasValue
            ? await _db.Categories.Where(x => x.Id == categoryId.Value).Select(x => x.Name).SingleAsync()
            : null;

        return new ProductResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            DiscountPercent = entity.DiscountPercent,
            HappyHourEnabled = entity.HappyHourEnabled,
            IsFavorite = entity.IsFavorite,
            CategoryId = entity.CategoryId,
            CategoryName = categoryName,
            FinalPrice = entity.HappyHourEnabled && entity.DiscountPercent > 0
                ? entity.Price * (1 - (entity.DiscountPercent / 100m))
                : entity.Price
        };
    }

    public async Task<ProductResponse> UpdateAsync(int restaurantUserId, int productId, ProductUpdateRequest request)
    {
        var entity = await _db.Products.SingleOrDefaultAsync(x => x.Id == productId && x.RestaurantUserId == restaurantUserId);
        if (entity is null) throw new KeyNotFoundException("Producto no encontrado.");

        int? categoryId = null;
        if (request.CategoryId.HasValue)
        {
            var catExists = await _db.Categories.AnyAsync(x => x.Id == request.CategoryId.Value && x.RestaurantUserId == restaurantUserId);
            if (!catExists) throw new InvalidOperationException("La categoría no existe o no pertenece a tu restaurante.");
            categoryId = request.CategoryId.Value;
        }

        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim();
        entity.Price = request.Price;
        entity.CategoryId = categoryId;
        entity.DiscountPercent = request.DiscountPercent;
        entity.HappyHourEnabled = request.HappyHourEnabled;
        entity.IsFavorite = request.IsFavorite;

        await _db.SaveChangesAsync();

        var categoryName = categoryId.HasValue
            ? await _db.Categories.Where(x => x.Id == categoryId.Value).Select(x => x.Name).SingleAsync()
            : null;

        return new ProductResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            DiscountPercent = entity.DiscountPercent,
            HappyHourEnabled = entity.HappyHourEnabled,
            IsFavorite = entity.IsFavorite,
            CategoryId = entity.CategoryId,
            CategoryName = categoryName,
            FinalPrice = entity.HappyHourEnabled && entity.DiscountPercent > 0
                ? entity.Price * (1 - (entity.DiscountPercent / 100m))
                : entity.Price
        };
    }

    public async Task DeleteAsync(int restaurantUserId, int productId)
    {
        var entity = await _db.Products.SingleOrDefaultAsync(x => x.Id == productId && x.RestaurantUserId == restaurantUserId);
        if (entity is null) throw new KeyNotFoundException("Producto no encontrado.");

        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<int> BulkUpdatePricesAsync(int restaurantUserId, BulkPriceUpdateRequest request)
    {
        var factor = 1 + (request.Percent / 100m);
        if (factor <= 0) throw new InvalidOperationException("El porcentaje resulta en precios inválidos.");

        var products = await _db.Products.Where(x => x.RestaurantUserId == restaurantUserId).ToListAsync();

        foreach (var p in products)
        {
            p.Price = decimal.Round(p.Price * factor, 2, MidpointRounding.AwayFromZero);
        }

        await _db.SaveChangesAsync();
        return products.Count;
    }

}
