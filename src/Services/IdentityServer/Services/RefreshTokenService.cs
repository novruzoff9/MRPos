using IdentityServer.Context;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Security.Cryptography;

namespace IdentityServer.Services;

public interface IRefreshTokenService
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    string GenerateRefreshToken();
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken, string replacedByToken);
    Task<RefreshToken> ValidateRefreshTokenAsync(string refreshToken, bool isTracking = false);
}

public class RefreshTokenService(IdentityDbContext context) : IRefreshTokenService
{
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64]; // 512-bit token
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<RefreshToken> ValidateRefreshTokenAsync(string refreshToken, bool isTracking = false)
    {
        IQueryable<RefreshToken> query = context.RefreshTokens;
        if (!isTracking)
            query = query.AsNoTracking();
        var token = await query.FirstOrDefaultAsync(x => x.Token == refreshToken);
        if (token == null)
            throw new NotFoundException("Refresh token not found");
        if (!token.IsValid)
            throw new UnauthorizedAccessException("Refresh token is invalid or expired");
        return token;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await ValidateRefreshTokenAsync(refreshToken, true);
        token.Revoke();
        await context.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, string replacedByToken)
    {
        var token = await ValidateRefreshTokenAsync(refreshToken, true);
        token.Revoke(replacedByToken);
        await context.SaveChangesAsync();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        bool exists = await context.RefreshTokens.AnyAsync(x => x.Token == refreshToken.Token);
        if (exists)
            throw new ConflictException("Refresh token already exists");
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
    }
}
