using System.Collections.Generic;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Data.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public IEnumerable<object> Categories => _db.Categories;

    public async Task<List<Category>> GetByRestaurantAsync(int restaurantUserId)
    {
        return await _db.Categories
            .Where(c => c.RestaurantUserId == restaurantUserId)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id, int restaurantUserId)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.RestaurantUserId == restaurantUserId);
    }

    public async Task AddAsync(Category category)
    {
        await _db.Categories.AddAsync(category);
    }

    public void Update(Category category)
    {
        _db.Categories.Update(category);
    }

    public void Remove(Category category)
    {
        _db.Categories.Remove(category);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}