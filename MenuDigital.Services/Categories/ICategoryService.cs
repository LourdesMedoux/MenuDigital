using MenuDigital.Common.DTOs.Categories;

namespace MenuDigital.Services.Categories;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetMineAsync(int restaurantUserId);
    Task<CategoryResponse> CreateAsync(int restaurantUserId, CategoryCreateRequest request);
    Task<CategoryResponse> UpdateAsync(int restaurantUserId, int categoryId, CategoryUpdateRequest request);
    Task DeleteAsync(int restaurantUserId, int categoryId);
}
