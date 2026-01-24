using MenuDigital.Common.DTOs.Auth;
using MenuDigital.Common.Entities;
using MenuDigital.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _db.RestaurantUsers.AnyAsync(x => x.Email.ToLower() == email);
        if (exists) throw new InvalidOperationException("El email ya está registrado.");

        var user = new RestaurantUser
        {
            RestaurantName = request.RestaurantName.Trim(),
            OwnerName = request.OwnerName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _db.RestaurantUsers.Add(user);
        await _db.SaveChangesAsync();

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

        var user = await _db.RestaurantUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
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
