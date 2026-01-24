using MenuDigital.Api.Security;
using MenuDigital.Common.DTOs.Account;
using MenuDigital.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [HttpPut]
    public async Task<ActionResult<AccountResponse>> Update(AccountUpdateRequest request)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            var result = await _service.UpdateAsync(userId, request);
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

    [HttpDelete]
    public async Task<IActionResult> Delete(AccountDeleteRequest request)
    {
        try
        {
            var userId = User.GetRestaurantUserId();
            await _service.DeleteAsync(userId, request);
            return NoContent();
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
}
