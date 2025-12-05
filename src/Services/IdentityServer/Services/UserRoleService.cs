using IdentityServer.Context;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace IdentityServer.Services;


public interface IUserRoleService
{
    Task<string> GetUserRole(string userId);
    Task<bool> AddUserToRoleAsync(string userId, string roleId);
    Task<bool> RemoveUserFromRole(string userId, string roleId);
}
public class UserRoleService(IdentityDbContext context) : IUserRoleService
{
    public async Task<bool> AddUserToRoleAsync(string userId, string roleId)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new NotFoundException("İstifadəçi tapılmadı");

        var roleExists = await context.Roles.AnyAsync(r => r.Id == roleId);
        if (!roleExists)
            throw new NotFoundException("Rol tapılmadı");

        var userCurrentRole = await context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId);
        if (userCurrentRole != null && userCurrentRole.RoleId == roleId)
            throw new ConflictException("İstifadəçi bu rola sahibdir");
        else
        {
            if (userCurrentRole != null)
                userCurrentRole.Revoke();
            var userRole = new UserRole(userId, roleId);
            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<string> GetUserRole(string userId)
    {
        var userRole = await context.UserRoles
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userId && !ur.Revoked.HasValue);
        if (userRole != null && userRole.Role != null)
            return userRole.Role.RoleName;
        else
            return string.Empty;
            //throw new NotFoundException("İstifadəçi rolu tapılmadı");
    }

    public async Task<bool> RemoveUserFromRole(string userId, string roleId)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new NotFoundException("İstifadəçi tapılmadı");
        var roleExists = await context.Roles.AnyAsync(r => r.Id == roleId);
        if (!roleExists)
            throw new NotFoundException("Rol tapılmadı");
        var userCurrentRole = context.UserRoles
            .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);
        if (userCurrentRole != null)
        {
            userCurrentRole.Revoke();
            await context.SaveChangesAsync();
            return true;
        }
        else
            throw new Exception("İstifadəçi belə bir rola sahib deyil");
    }

}
