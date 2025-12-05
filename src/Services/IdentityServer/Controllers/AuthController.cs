using IdentityServer.DTOs;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            return BadRequest("Email and password are required.");

        var token = await authenticationService.LoginAsync(loginDto.Email, loginDto.Password);
        var response = Response<TokenResponseDto>.Success(token, 200);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (string.IsNullOrEmpty(refreshTokenDto.RefreshToken))
            return BadRequest("Refresh token is required.");
        var token = await authenticationService.RefreshAsync(refreshTokenDto.RefreshToken);
        var response = Response<TokenResponseDto>.Success(token, 200);
        return Ok(response);
    }
}
