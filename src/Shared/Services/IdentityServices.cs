using Microsoft.AspNetCore.Http;
using Shared.Interfaces;
using System.Security.Claims;

namespace Shared.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public ClaimsPrincipal GetUser => httpContextAccessor.HttpContext.User;

    public string GetUserId => GetClaimValue("sub");
    //public string GetCompanyId => GetClaimValue("company");
    public string GetCompanyId => "693100be-62f4-4d4c-9fb0-521300fa7396";
    public string GetRole => GetClaimValue("roles");
    public string GetBranchId => GetClaimValue("branch");

    private string GetClaimValue(string claimType)
    {
        var claim = httpContextAccessor.HttpContext?.User?.FindFirst(claimType);
        return claim?.Value ?? throw new Exception("Token-de problem var");
    }
}
