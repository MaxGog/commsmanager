using Microsoft.AspNetCore.Mvc;
using CommsManager.Application.DTOs.Auth;
using CommsManager.Application.Interfaces;

namespace CommsManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _auth.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("promote/{userId:guid}")]
    public async Task<IActionResult> Promote(Guid userId)
    {
        await _auth.PromoteToCreatorAsync(userId);
        return NoContent();
    }
}
