using IdentityServer.DTOs.User;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISharedIdentityService _identityService;

        public UsersController(UserManager<ApplicationUser> userManager, ISharedIdentityService identityService)
        {
            _userManager = userManager;
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                CompanyId = request.CompanyId                
            };

            var result = await _userManager.CreateAsync(user, request.Password);

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

        [HttpGet("WithToken")]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaims = User.Claims.FirstOrDefault(x => x.Type == "sub");

            if (userIdClaims == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userIdClaims.Value);

            if (user == null)
            {
                return BadRequest();
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CompanyId = user.CompanyId,
                Roles = roles.ToList() ?? Array.Empty<string>().ToList()
            });
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
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


            return Ok(userslist);
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] SignUpDto request)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                CompanyId = _identityService.GetCompanyId
            };

            var result = await _userManager.CreateAsync(user, request.Password);

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
