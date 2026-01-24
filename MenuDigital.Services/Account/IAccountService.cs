using MenuDigital.Common.DTOs.Account;

namespace MenuDigital.Services.Account;

public interface IAccountService
{
    Task<AccountResponse> UpdateAsync(int restaurantUserId, AccountUpdateRequest request);
    Task DeleteAsync(int restaurantUserId, AccountDeleteRequest request);
}
