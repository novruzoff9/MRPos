using FluentValidation;
using FluentValidation.Results;
using IdentityServer.DTOs;
using IdentityServer.Services;
using IdentityServer.Validations;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultTypes;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IUserService userService, 
    IUserRoleService userRoleService
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto)
    {
        var user = await userService.CreateUser(userDto);
        var response = Response<UserDto>.Success(user, 200);
        return Ok(response);
    }

    [HttpPost("assignrole")]
    public async Task<IActionResult> AssignUserToRole(AssignUserToRoleDto dto)
    {
        var result = await userRoleService.AddUserToRoleAsync(dto.UserId, dto.RoleId);
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto userDto)
    {
        var updatedUser = await userService.UpdateUser(userId, userDto);
        var response = Response<UserDto>.Success(updatedUser, 200);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsersAync();
        var response = Response<IEnumerable<UserDto>>.Success(users, 200);
        return Ok(response);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var result = await userService.ChangePassword(changePasswordDto);
        var response = Response<bool>.Success(result, 200);
        return Ok(response);
    }
}
