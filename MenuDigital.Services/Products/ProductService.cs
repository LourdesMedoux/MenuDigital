using MenuDigital.Common.DTOs.Products;
using MenuDigital.Common.DTOs.ProductsExtras;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MenuDigital.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ProductResponse>> GetMineAsync(int restaurantUserId, int? categoryId, bool? onlyFavorites, bool? onlyDiscounted)
    {
        var products = await _repository.GetByRestaurantAsync(restaurantUserId);
        var query = products.AsQueryable();

        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (onlyFavorites == true) query = query.Where(x => x.IsFavorite);
        if (onlyDiscounted == true) query = query.Where(x => x.HappyHourEnabled && x.DiscountPercent > 0);

        var list = query
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
            .ToList();

        return list;
    }

    public async Task<ProductResponse> CreateAsync(int restaurantUserId, ProductCreateRequest request)
    {
        int? categoryId = null;

        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, restaurantUserId);
            if (category is null) throw new InvalidOperationException("La categoría no existe o no pertenece a tu restaurante.");
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

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        var categoryName = categoryId.HasValue
            ? (await _categoryRepository.GetByIdAsync(categoryId.Value, restaurantUserId))?.Name
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
        var entity = await _repository.GetByIdAsync(productId, restaurantUserId);
        if (entity is null) throw new KeyNotFoundException("Producto no encontrado.");

        int? categoryId = null;
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, restaurantUserId);
            if (category is null) throw new InvalidOperationException("La categoría no existe o no pertenece a tu restaurante.");
            categoryId = request.CategoryId.Value;
        }

        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim();
        entity.Price = request.Price;
        entity.CategoryId = categoryId;
        entity.DiscountPercent = request.DiscountPercent;
        entity.HappyHourEnabled = request.HappyHourEnabled;
        entity.IsFavorite = request.IsFavorite;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        var categoryName = categoryId.HasValue
            ? (await _categoryRepository.GetByIdAsync(categoryId.Value, restaurantUserId))?.Name
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
        var entity = await _repository.GetByIdAsync(productId, restaurantUserId);
        if (entity is null) throw new KeyNotFoundException("Producto no encontrado.");

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task<int> BulkUpdatePricesAsync(int restaurantUserId, BulkPriceUpdateRequest request)
    {
        var factor = 1 + (request.Percent / 100m);
        if (factor <= 0) throw new InvalidOperationException("El porcentaje resulta en precios inválidos.");

        var products = await _repository.GetByRestaurantAsync(restaurantUserId);

        foreach (var p in products)
        {
            p.Price = decimal.Round(p.Price * factor, 2, MidpointRounding.AwayFromZero);
            _repository.Update(p);
        }

        await _repository.SaveChangesAsync();
        return products.Count;
    }

}
