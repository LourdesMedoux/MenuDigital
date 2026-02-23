using MenuDigital.Common.Entities;

namespace MenuDigital.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetByRestaurantAsync(int restaurantUserId);
    Task<Product?> GetByIdAsync(int id, int restaurantUserId);
    Task<List<Product>> GetByCategoryAsync(int categoryId, int restaurantUserId);
    Task AddAsync(Product product);
    void Update(Product product);
    void Remove(Product product);
    Task SaveChangesAsync();
}