using MenuDigital.Common.DTOs.Account;
using MenuDigital.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Account;

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;

    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AccountResponse> UpdateAsync(int restaurantUserId, AccountUpdateRequest request)
    {
        var user = await _db.RestaurantUsers.SingleOrDefaultAsync(x => x.Id == restaurantUserId);
        if (user is null) throw new KeyNotFoundException("Cuenta no encontrada.");

        var ok = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
        if (!ok) throw new InvalidOperationException("Contraseña actual incorrecta.");

        var email = request.Email.Trim().ToLowerInvariant();

        var emailTaken = await _db.RestaurantUsers.AnyAsync(x => x.Id != restaurantUserId && x.Email.ToLower() == email);
        if (emailTaken) throw new InvalidOperationException("El email ya está registrado.");

        user.RestaurantName = request.RestaurantName.Trim();
        user.OwnerName = request.OwnerName.Trim();
        user.Email = email;

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        }

        await _db.SaveChangesAsync();

        return new AccountResponse
        {
            Id = user.Id,
            RestaurantName = user.RestaurantName,
            OwnerName = user.OwnerName,
            Email = user.Email
        };
    }

    public async Task DeleteAsync(int restaurantUserId, AccountDeleteRequest request)
    {
        var user = await _db.RestaurantUsers.SingleOrDefaultAsync(x => x.Id == restaurantUserId);
        if (user is null) throw new KeyNotFoundException("Cuenta no encontrada.");

        var ok = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
        if (!ok) throw new InvalidOperationException("Contraseña actual incorrecta.");

        _db.RestaurantUsers.Remove(user);
        await _db.SaveChangesAsync();
    }
}
