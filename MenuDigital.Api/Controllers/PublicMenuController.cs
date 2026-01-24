using MenuDigital.Common.DTOs.Public;
using MenuDigital.Services.Public;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Controllers;

[ApiController]
[Route("api/public")]
public class PublicMenuController : ControllerBase
{
    private readonly IPublicMenuService _service;

    public PublicMenuController(IPublicMenuService service)
    {
        _service = service;
    }

    [HttpGet("restaurants")]
    public async Task<ActionResult<List<PublicRestaurantResponse>>> GetRestaurants()
    {
        var result = await _service.GetRestaurantsAsync();
        return Ok(result);
    }

    [HttpGet("restaurants/{restaurantId:int}/menu")]
    public async Task<ActionResult<List<PublicProductResponse>>> GetMenu(
        int restaurantId,
        [FromQuery] int? categoryId,
        [FromQuery] bool? onlyFavorites,
        [FromQuery] bool? onlyDiscounted)
    {
        var result = await _service.GetMenuAsync(restaurantId, categoryId, onlyFavorites, onlyDiscounted);
        return Ok(result);
    }
}
