using MenuDigital.Common.DTOs.Account;
using MenuDigital.Data.Data;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Account;

public class AccountService : IAccountService
{
    private readonly IRestaurantUserRepository _repository;

    public AccountService(IRestaurantUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<AccountResponse> UpdateAsync(int restaurantUserId, AccountUpdateRequest request)
    {
        var user = await _repository.GetByIdAsync(restaurantUserId);
        if (user is null) throw new KeyNotFoundException("Cuenta no encontrada.");

        var ok = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
        if (!ok) throw new InvalidOperationException("Contraseña actual incorrecta.");

        var email = request.Email.Trim().ToLowerInvariant();

        var emailTaken = await _repository.ExistsByEmailExceptAsync(restaurantUserId, email);
        if (emailTaken) throw new InvalidOperationException("El email ya está registrado.");

        user.RestaurantName = request.RestaurantName.Trim();
        user.OwnerName = request.OwnerName.Trim();
        user.Email = email;

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        }

        await _repository.SaveChangesAsync();

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
        var user = await _repository.GetByIdAsync(restaurantUserId);
        if (user is null) throw new KeyNotFoundException("Cuenta no encontrada.");

        var ok = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
        if (!ok) throw new InvalidOperationException("Contraseña actual incorrecta.");

        await _repository.RemoveAsync(user);
        await _repository.SaveChangesAsync();
    }
}
