using MenuDigital.Common.DTOs.Auth;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using MenuDigital.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IRestaurantUserRepository _repository;
    private readonly ITokenService _tokenService;

    public AuthService(
    IRestaurantUserRepository repository,
    ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _repository.ExistsByEmailAsync(email);
        if (exists) throw new InvalidOperationException("El email ya está registrado.");

        var user = new RestaurantUser
        {
            RestaurantName = request.RestaurantName.Trim(),
            OwnerName = request.OwnerName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        var token = _tokenService.CreateToken(user);

        return new AuthResponse
        {
            RestaurantUserId = user.Id,
            RestaurantName = user.RestaurantName,
            Token = token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _repository.GetByEmailAsync(email);
        if (user is null) throw new InvalidOperationException("Credenciales inválidas.");

        var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!ok) throw new InvalidOperationException("Credenciales inválidas.");

        var token = _tokenService.CreateToken(user);

        return new AuthResponse
        {
            RestaurantUserId = user.Id,
            RestaurantName = user.RestaurantName,
            Token = token
        };
    }
}
