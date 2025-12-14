using IdentityServer.DTOs;
using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.General;
using Shared.ResultTypes;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleDto role)
    {
        var result = await roleService.CreateRoleAsync(role);
        if (result is not null)
        {
            var response = Response<IdentityRole>.Success(result, 200);
            return Ok(response);
        }
        else
            return BadRequest("Failed to create role.");
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await roleService.GetRolesAsync();
        var response = Response<IEnumerable<IdentityRole>>.Success(roles, 200);
        return Ok(response);
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> GetRolesLookup()
    {
        var rolesLookup = await roleService.GetLookupAsync<IdentityRole, string>("RoleName");
        var response = Response<List<LookupDto<string>>>.Success(rolesLookup, 200);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var result = await roleService.DeleteRoleAsync(id);
        if (result)
        {
            var response = Response<NoContent>.Success(204);
            return Ok(response);
        }
        else
            return NotFound("Role not found.");
    }
}
