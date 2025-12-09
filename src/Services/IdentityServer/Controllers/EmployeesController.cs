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
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await employeeService.GetEmployeesAsync();
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
