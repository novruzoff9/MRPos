using Azure.Core;
using IdentityServer.Configurations;
using IdentityServer.DTOs;
using IdentityServer.Helpers;
using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Services;

public interface IAuthenticationService
{
    Task<TokenResponseDto> LoginAsync(string email, string password);
    Task<TokenResponseDto> RefreshAsync(string refreshToken, string? ip = null, string? device = null);
}

public class AuthenticationService(IUserService userService, IRefreshTokenService refreshTokenService, JwtTokenGenerator jwtTokenGenerator) : IAuthenticationService
{
    public async Task<TokenResponseDto> LoginAsync(string email, string password)
    {
        var user = await userService.GetUserByEmailAsync(email);
        var pass = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
        if (!pass)
            throw new UnauthorizedAccessException("Şifrə yanlışdır");
        var (accessToken, expiryMinutes) = jwtTokenGenerator.GenerateToken(user);
        var refreshToken = refreshTokenService.GenerateRefreshToken();
        RefreshToken refreshTokenEntity = new(refreshToken, DateTime.UtcNow.AddDays(30), user.Id);
        await refreshTokenService.AddRefreshTokenAsync(refreshTokenEntity);
        TokenResponseDto tokenResponse = new()
        {
            access_token = accessToken,
            expires_in = expiryMinutes,
            token_type = "Bearer",
            refresh_token = refreshToken,
            scope = string.Empty
        };
        return tokenResponse;
    }

    public async Task<TokenResponseDto> RefreshAsync(string refreshToken, string? ip = null, string? device = null)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
        // Validate the refresh token
        var refreshTokenEntity = await refreshTokenService.ValidateRefreshTokenAsync(refreshToken);
        // Fetch the user associated with the refresh token
        var user = await userService.GetUserByIdAsync(refreshTokenEntity.UserId);
        // Generate a new access token for the user
        var (accessToken, expiryMinutes) = jwtTokenGenerator.GenerateToken(user);
        // Revoke the old refresh token
        await refreshTokenService.RevokeRefreshTokenAsync(refreshToken, accessToken);
        // Create a new refresh token
        string newRefreshToken = refreshTokenService.GenerateRefreshToken();
        RefreshToken newRefreshTokenEntity = new(newRefreshToken, DateTime.UtcNow.AddDays(30), user.Id);
        await refreshTokenService.AddRefreshTokenAsync(newRefreshTokenEntity);

        TokenResponseDto tokenResponse = new()
        {
            access_token = accessToken,
            expires_in = expiryMinutes,
            token_type = "Bearer",
            refresh_token = newRefreshToken,
            scope = string.Empty
        };

        return tokenResponse;
    }
}
