namespace IdentityServer.Models;

public class UserRole(string userId, string roleId)
{
    public string UserId { get; private set; } = userId;
    public IdentityUser? User { get; set; }

    public string RoleId { get; private set; } = roleId;
    public IdentityRole? Role { get; set; }

    public DateTime Assigned { get; private set; } = DateTime.UtcNow;
    public DateTime? Revoked { get; private set; }

    public void Revoke()
    {
        Revoked = DateTime.UtcNow;
    }
}