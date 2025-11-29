using IdentityServer.Configurations;
using IdentityServer.DTOs;
using IdentityServer.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Helpers;

public class JwtTokenGenerator(IOptions<JwtSettings> options)
{
    private readonly JwtSettings _jwtSettings = options.Value;

    public (string, int) GenerateToken(UserDetailedDto user, List<Claim>? extraClaims = null)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id),
            new("email", user.Email),
            new("fullName", $"{user.FirstName} {user.LastName}"),
            new("company", user.CompanyId),
            new("branch", user.BranchId)
        };
        if (extraClaims != null)
            claims.AddRange(extraClaims);

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim("role", role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return (accessToken, _jwtSettings.ExpiryMinutes);
    }
}
