using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Data.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetByRestaurantAsync(int restaurantUserId)
    {
        return await _db.Products
            .Where(p => p.RestaurantUserId == restaurantUserId)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id, int restaurantUserId)
    {
        return await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.RestaurantUserId == restaurantUserId);
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId, int restaurantUserId)
    {
        return await _db.Products
            .Where(p => p.CategoryId == categoryId && p.RestaurantUserId == restaurantUserId)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _db.Products.AddAsync(product);
    }

    public void Update(Product product)
    {
        _db.Products.Update(product);
    }

    public void Remove(Product product)
    {
        _db.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}