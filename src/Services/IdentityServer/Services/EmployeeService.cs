using IdentityServer.Context;
using IdentityServer.DTOs.Employee;
using IdentityServer.Grpc.Interfaces;
using IdentityServer.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace IdentityServer.Services;

public interface IEmployeeService
{
    Task<ICollection<EmployeeReturnDto>> GetEmployeesAsync(EmployeeFilterDto? filter = null);
    Task<bool> CreateEmployeeAsync(EmployeeCreateDto createDto);
}

public class EmployeeService(
    IdentityDbContext dbContext,
    IIdentityService identityService,
    IRoleService roleService,
    IUserRoleService userRoleService,
    IMapper mapper,
    IBranchesGrpc branchesGrpc
    ) : IEmployeeService
{
    public async Task<bool> CreateEmployeeAsync(EmployeeCreateDto createDto)
    {
        string roleId = createDto.RoleId;
        var role = await roleService.GetRoleByIdAsync(roleId);
        createDto = createDto with { CompanyId = identityService.GetCompanyId };
        var user = mapper.Map<IdentityUser>(createDto);
        using var transaction = await dbContext.Database.BeginTransactionAsync();

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        await userRoleService.AddUserToRoleAsync(user.Id, role.Id);

        await transaction.CommitAsync();
        return true;

    }

    public async Task<ICollection<EmployeeReturnDto>> GetEmployeesAsync(EmployeeFilterDto? filter = null)
    {
        var query = dbContext.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .Where(x => x.CompanyId == identityService.GetCompanyId);

        // Filter by BranchId if provided
        if (!string.IsNullOrWhiteSpace(filter?.BranchId))
        {
            query = query.Where(x => x.BranchId == filter.BranchId);
        }

        // Filter by RoleId if provided
        if (!string.IsNullOrWhiteSpace(filter?.RoleId))
        {
            query = query.Where(x => x.Roles.Any(r => r.RoleId == filter.RoleId));
        }

        var employees = await query
            .Select(e => 
            new { e.Id, Name = $"{e.FirstName} {e.LastName}", e.Email, Role = e.Roles.First().Role!.RoleName, e.BranchId })
            .ToListAsync();

        var branchIds = employees.Select(e => e.BranchId).Distinct().ToList();
        
        // gRPC request - branch adlarını almaq üçün
        var branchNames = await branchesGrpc.GetBranchNamesAsync(branchIds);
        
        // Branch adlarını employee-lərə əlavə edək
        var result = employees.Select(e => new EmployeeReturnDto(
            e.Id,
            e.Name,
            e.Email,
            e.Role,
            branchNames.TryGetValue(e.BranchId, out var branchName) ? branchName : e.BranchId
        )).ToList();

        return result;
    }
}