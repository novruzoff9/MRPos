using IdentityServer.DTOs.User;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] SignUpDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
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

        [HttpGet]
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

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Roles = roles ?? Array.Empty<string>()
            });
        }
    }
}
