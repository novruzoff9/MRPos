using FluentValidation;
using IdentityServer.Context;
using IdentityServer.DTOs;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Models.General;

namespace IdentityServer.Services;


public interface IRoleService
{
    Task<IdentityRole> CreateRoleAsync(CreateRoleDto request);
    Task<List<IdentityRole>> GetRolesAsync();
    Task<bool> DeleteRoleAsync(string id);
    Task<IdentityRole> GetRoleByIdAsync(string id);
    Task<List<LookupDto<TId>>> GetLookupAsync<TEntity, TId>(string name = "Name") where TEntity : class;
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

    public async Task<List<LookupDto<TId>>> GetLookupAsync<TEntity, TId>(string name = "Name") where TEntity : class
    {
        var dbSet = context.Set<TEntity>();

        var idProp = typeof(TEntity).GetProperty("Id");
        var nameProp = typeof(TEntity).GetProperty(name);

        if (idProp == null || nameProp == null)
            throw new Exception($"{typeof(TEntity).Name} entity-də Id və Name property mövcud deyil.");

        return await dbSet
            .Select(e => new LookupDto<TId>(
                (TId)idProp.GetValue(e),
                (string)nameProp.GetValue(e)
            ))
            .ToListAsync();
    }

    public async Task<IdentityRole> GetRoleByIdAsync(string id)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (role == null)
            throw new NotFoundException("Rol tapılmadı");
        return role;
    }

    public async Task<List<IdentityRole>> GetRolesAsync()
    {
        return await context.Roles.ToListAsync();
    }
}
