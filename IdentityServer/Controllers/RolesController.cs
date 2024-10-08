using IdentityServer.DTOs.Role;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            roleName = roleName.ToLower();
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("Role already exists");
            }

            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleId)
        {
            // Kullanıcıyı bulalım
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Rolü bulalım
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            // Kullanıcıyı role atayalım
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User assigned to role successfully");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }
    }
}
