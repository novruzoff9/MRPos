using IdentityServer.Data;
using IdentityServer.DTOs.User;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.ResultTypes;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(string companyId)
        {
            var users = await _userManager.Users.Where(x => x.CompanyId == companyId).ToListAsync();
            var userslist = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userslist.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    CompanyId = user.CompanyId,
                    Roles = roles.ToList() ?? Array.Empty<string>().ToList()
                });
            }

            var result = Response<List<UserDto>>.Success(userslist, 200);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateUserDto request)
        {
            var employee = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                //CompanyId = _identityService.GetCompanyId
            };

            var result = await _userManager.CreateAsync(employee, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Ok(new
                {
                    Errors = errors
                });
            }

            return Ok();
        }
    }
}
