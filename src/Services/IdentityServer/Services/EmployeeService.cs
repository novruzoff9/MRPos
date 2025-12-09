using IdentityServer.Context;
using IdentityServer.DTOs.Employee;
using IdentityServer.Grpc.Interfaces;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace IdentityServer.Services;

public interface IEmployeeService
{
    Task<ICollection<EmployeeReturnDto>> GetEmployeesAsync();
    Task<bool> CreateEmployeeAsync(EmployeeCreateDto createDto);
}

public class EmployeeService(
    IdentityDbContext dbContext,
    IIdentityService identityService,
    IBranchesGrpc branchesGrpc
    ) : IEmployeeService
{
    public async Task<bool> CreateEmployeeAsync(EmployeeCreateDto createDto)
    {
        var user = new IdentityUser(createDto.FirstName, createDto.LastName, createDto.Email, createDto.Password, createDto.RoleId, identityService.GetCompanyId);
        await dbContext.Users.AddAsync(user);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<ICollection<EmployeeReturnDto>> GetEmployeesAsync()
    {
        var employees = await dbContext.Users
            .Where(x=>x.CompanyId == identityService.GetCompanyId)
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
