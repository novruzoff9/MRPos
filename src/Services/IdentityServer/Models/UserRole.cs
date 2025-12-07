namespace IdentityServer.Models;

public class UserRole
{
    public string UserId { get; private set; }
    public IdentityUser? User { get; protected set; }

    public string RoleId { get; private set; }
    public IdentityRole? Role { get; protected set; }

    public DateTime Assigned { get; private set; }
    public DateTime? Revoked { get; private set; }

    private UserRole() { }

    public UserRole(string userId, string roleId)
    {
        UserId = userId;
        RoleId = roleId;
        Assigned = DateTime.UtcNow;
    }

    public void Revoke()
    {
        Revoked = DateTime.UtcNow;
    }
}