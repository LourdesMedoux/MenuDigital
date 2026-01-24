using MenuDigital.Common.Entities;

namespace MenuDigital.Services.Auth;

public interface ITokenService
{
    string CreateToken(RestaurantUser user);
}
