using IdentityServer.DTOs.Employee;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmployees([FromQuery] string? branchId = null, [FromQuery] string? roleId = null)
    {
        var filter = new EmployeeFilterDto(branchId, roleId);
        var employees = await employeeService.GetEmployeesAsync(filter);
        var response = Response<ICollection<EmployeeReturnDto>>.Success(employees, 200);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeCreateDto createDto)
    {
        var result = await employeeService.CreateEmployeeAsync(createDto);
        var response = Response<bool>.Success(result, 201);
        return Ok(response);
    }
}
