using IdentityServer.DTOs;
using IdentityServer.Exceptions;
using IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var token = await authenticationService.LoginAsync(loginDto.Email, loginDto.Password);
        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (string.IsNullOrEmpty(refreshTokenDto.RefreshToken))
        {
            return BadRequest("Refresh token is required.");
        }
        var token = await authenticationService.RefreshAsync(refreshTokenDto.RefreshToken);
        return Ok(token);
    }
}
