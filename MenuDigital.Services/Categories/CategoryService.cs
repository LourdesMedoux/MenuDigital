using MenuDigital.Common.DTOs.Categories;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Repositories.Interfaces;
using System;
using System.Linq;

namespace MenuDigital.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryResponse>> GetMineAsync(int restaurantUserId)
    {
        var categories = await _repository.GetByRestaurantAsync(restaurantUserId);

        return categories
            .OrderBy(x => x.Name)
            .Select(x => new CategoryResponse { Id = x.Id, Name = x.Name })
            .ToList();
    }

    public async Task<CategoryResponse> CreateAsync(int restaurantUserId, CategoryCreateRequest request)
    {
        var name = request.Name.Trim();

        var categories = await _repository.GetByRestaurantAsync(restaurantUserId);

        var exists = categories.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

        if (exists) throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

        var entity = new Category
        {
            Name = name,
            RestaurantUserId = restaurantUserId
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return new CategoryResponse { Id = entity.Id, Name = entity.Name };
    }

    public async Task<CategoryResponse> UpdateAsync(int restaurantUserId, int categoryId, CategoryUpdateRequest request)
    {
        var entity = await _repository.GetByIdAsync(categoryId, restaurantUserId);

        if (entity is null) throw new KeyNotFoundException("Categoría no encontrada.");

        var name = request.Name.Trim();

        var categories = await _repository.GetByRestaurantAsync(restaurantUserId);

        var exists = categories.Any(x => x.Id != categoryId && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

        if (exists) throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

        entity.Name = name;
        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return new CategoryResponse { Id = entity.Id, Name = entity.Name };
    }

    public async Task DeleteAsync(int restaurantUserId, int categoryId)
    {
        var entity = await _repository.GetByIdAsync(categoryId, restaurantUserId);

        if (entity is null) throw new KeyNotFoundException("Categoría no encontrada.");

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
    }
}
