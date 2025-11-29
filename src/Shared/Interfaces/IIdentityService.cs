using System.Security.Claims;

namespace Shared.Interfaces;

public interface IIdentityService
{
    string GetRole { get; }
    string GetUserId { get; }
    string GetCompanyId { get; }
    string GetBranchId { get; }
    ClaimsPrincipal GetUser { get; }
}