using MenuDigital.Api.Security;
using MenuDigital.Common.DTOs.Categories;
using MenuDigital.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetMine()
    {
        var userId = User.GetRestaurantUserId();
        var result = await _service.GetMineAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            var result = await _service.CreateAsync(userId, request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> Update(int id, CategoryUpdateRequest request)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            var result = await _service.UpdateAsync(userId, id, request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            await _service.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
