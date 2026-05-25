using Microsoft.AspNetCore.Mvc;

namespace CommsManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> HandleResponse<T>(T data, string? errorMessage = null)
    {
        if (errorMessage != null)
            return BadRequest(new { message = errorMessage });

        return Ok(new { data });
    }
}
