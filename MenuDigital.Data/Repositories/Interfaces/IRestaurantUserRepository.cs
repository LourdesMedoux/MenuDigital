using MenuDigital.Common.Entities;

namespace MenuDigital.Data.Repositories.Interfaces;

public interface IRestaurantUserRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<RestaurantUser?> GetByEmailAsync(string email);
    Task AddAsync(RestaurantUser user);
    Task SaveChangesAsync();
    Task<RestaurantUser?> GetByIdAsync(int id);
    Task<bool> ExistsByEmailExceptAsync(int excludeId, string email);
    Task RemoveAsync(RestaurantUser user);
    Task<List<RestaurantUser>> GetAllAsync();
}