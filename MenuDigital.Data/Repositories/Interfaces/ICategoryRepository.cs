using MenuDigital.Common.Entities;

namespace MenuDigital.Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    IEnumerable<object> Categories { get; }

    Task<List<Category>> GetByRestaurantAsync(int restaurantUserId);
    Task<Category?> GetByIdAsync(int id, int restaurantUserId);
    Task AddAsync(Category category);
    void Update(Category category);
    void Remove(Category category);
    Task SaveChangesAsync();
}