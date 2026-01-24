using MenuDigital.Common.DTOs.Categories;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryResponse>> GetMineAsync(int restaurantUserId)
    {
        return await _db.Categories
            .Where(x => x.RestaurantUserId == restaurantUserId)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryResponse { Id = x.Id, Name = x.Name })
            .ToListAsync();
    }

    public async Task<CategoryResponse> CreateAsync(int restaurantUserId, CategoryCreateRequest request)
    {
        var name = request.Name.Trim();

        var exists = await _db.Categories.AnyAsync(x =>
            x.RestaurantUserId == restaurantUserId &&
            x.Name.ToLower() == name.ToLower());

        if (exists) throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

        var entity = new Category
        {
            Name = name,
            RestaurantUserId = restaurantUserId
        };

        _db.Categories.Add(entity);
        await _db.SaveChangesAsync();

        return new CategoryResponse { Id = entity.Id, Name = entity.Name };
    }

    public async Task<CategoryResponse> UpdateAsync(int restaurantUserId, int categoryId, CategoryUpdateRequest request)
    {
        var entity = await _db.Categories.SingleOrDefaultAsync(x =>
            x.Id == categoryId && x.RestaurantUserId == restaurantUserId);

        if (entity is null) throw new KeyNotFoundException("Categoría no encontrada.");

        var name = request.Name.Trim();

        var exists = await _db.Categories.AnyAsync(x =>
            x.RestaurantUserId == restaurantUserId &&
            x.Id != categoryId &&
            x.Name.ToLower() == name.ToLower());

        if (exists) throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

        entity.Name = name;
        await _db.SaveChangesAsync();

        return new CategoryResponse { Id = entity.Id, Name = entity.Name };
    }

    public async Task DeleteAsync(int restaurantUserId, int categoryId)
    {
        var entity = await _db.Categories.SingleOrDefaultAsync(x =>
            x.Id == categoryId && x.RestaurantUserId == restaurantUserId);

        if (entity is null) throw new KeyNotFoundException("Categoría no encontrada.");

        _db.Categories.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
