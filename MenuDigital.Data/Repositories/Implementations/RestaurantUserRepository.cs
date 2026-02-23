using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Data.Repositories.Implementations;

public class RestaurantUserRepository : IRestaurantUserRepository
{
    private readonly AppDbContext _db;

    public RestaurantUserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _db.RestaurantUsers
            .AnyAsync(x => x.Email.ToLower() == email);
    }

    public async Task<RestaurantUser?> GetByEmailAsync(string email)
    {
        return await _db.RestaurantUsers
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);
    }

    public async Task AddAsync(RestaurantUser user)
    {
        await _db.RestaurantUsers.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<RestaurantUser?> GetByIdAsync(int id)
    {
        return await _db.RestaurantUsers
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByEmailExceptAsync(int excludeId, string email)
    {
        return await _db.RestaurantUsers
            .AnyAsync(x => x.Id != excludeId && x.Email.ToLower() == email);
    }

    public Task RemoveAsync(RestaurantUser user)
    {
        _db.RestaurantUsers.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<List<RestaurantUser>> GetAllAsync()
    {
        return await _db.RestaurantUsers.ToListAsync();
    }
}