using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTOs;
using ProjectManagementAPI.Services;

namespace ProjectManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var token = await _authService.RegisterAsync(dto);
        
        if (token == null)
            return BadRequest(new { message = "Username already exists" });
            
        return Ok(new { token });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        
        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });
            
        return Ok(new { token });
    }
}
