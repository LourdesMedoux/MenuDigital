using MenuDigital.Api.Security;
using MenuDigital.Common.DTOs.Products;
using MenuDigital.Common.DTOs.ProductsExtras;
using MenuDigital.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetMine([FromQuery] int? categoryId, [FromQuery] bool? onlyFavorites, [FromQuery] bool? onlyDiscounted)
    {
        var userId = User.GetRestaurantUserId();
        var result = await _service.GetMineAsync(userId, categoryId, onlyFavorites, onlyDiscounted);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(ProductCreateRequest request)
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
    public async Task<ActionResult<ProductResponse>> Update(int id, ProductUpdateRequest request)
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

    [HttpPatch("bulk-price")]
    public async Task<IActionResult> BulkPrice(BulkPriceUpdateRequest request)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            var updated = await _service.BulkUpdatePricesAsync(userId, request);
            return Ok(new { updated });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
