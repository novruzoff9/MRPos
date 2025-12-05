using FluentValidation;
using IdentityServer.Context;
using IdentityServer.DTOs;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace IdentityServer.Services;


public interface IRoleService
{
    Task<IdentityRole> CreateRoleAsync(CreateRoleDto request);
    Task<List<IdentityRole>> GetRolesAsync();
    Task<bool> DeleteRoleAsync(string id);
}

public class RoleService(IdentityDbContext context, IValidator<CreateRoleDto> createRoleValidator) : IRoleService
{
    public async Task<IdentityRole> CreateRoleAsync(CreateRoleDto request)
    {
        var valResult = createRoleValidator.Validate(request);
        if (!valResult.IsValid)
            throw new ValidationException(valResult.Errors);
        string normalizedRoleName = request.Name.Trim().ToLower();
        var role = new IdentityRole(request.Name);
        var existingRole = await context.Roles.AnyAsync(r => r.NormalizedName == normalizedRoleName);
        if (existingRole)
            throw new ConflictException("Rol artıq mövcuddur");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> DeleteRoleAsync(string id)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (role == null)
            throw new NotFoundException("Rol tapılmadı");
        context.Roles.Remove(role);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<IdentityRole>> GetRolesAsync()
    {
        return await context.Roles.ToListAsync();
    }
}
