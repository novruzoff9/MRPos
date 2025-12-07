namespace IdentityServer.Models;

public class IdentityRole(string roleName)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string RoleName { get; private set; } = roleName;
    public string NormalizedName { get; private set; } = roleName.Trim().ToLower();
    public ICollection<UserRole> Users { get; protected set; } = new List<UserRole>();
}
