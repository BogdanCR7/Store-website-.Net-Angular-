
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly  ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManage;
        private readonly JwtSettings _jwtSettings;
        public AuthController(ApplicationContext applicationContext,
             IOptionsSnapshot<JwtSettings> jwtSettings,
             UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManage = roleManager;
            _applicationContext = applicationContext;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpViemodel userLoginResource)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userLoginResource.Email);
            if (user is not null)
            {
                return BadRequest("User is already exist");
            }
            User newuser = new User() { UserName = userLoginResource.Email,City=userLoginResource.City,Address=userLoginResource.Address,
                PhoneNumber=userLoginResource.PhoneNumber,FirstName=userLoginResource.Name
            };
            var userSigninResult = await _userManager.CreateAsync(newuser, userLoginResource.Password);
            
            if (userSigninResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(newuser);
                return Ok(new
                {
                    access_token = GenerateJwt(newuser, roles)
                });
            }

            return BadRequest("Email or password incorrect.");
        }
        [HttpPost("Roles")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should be provided.");
            }

            var newRole = new Role
            {
                Name = roleName
            };

            var roleResult = await _roleManage.CreateAsync(newRole);

            if (roleResult.Succeeded)
            {
                return Ok();
            }

            return Problem(roleResult.Errors.First().Description, null, 500);
        }

        [HttpPost("UserRole")]
        public async Task<IActionResult> AddUserToRole(AddUserToRole addUserToRole)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == addUserToRole.UserEmail);

            var result = await _userManager.AddToRoleAsync(user, addUserToRole.RoleName);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem(result.Errors.First().Description, null, 500);
        }
        [HttpPost]
        [Route("[action]")] 
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest login)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == login.Email);
            if (user is null)
            {
                return NotFound("User not found");
            }

            var userSigninResult = await _userManager.CheckPasswordAsync(user, login.Password);

            if (userSigninResult)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(new
                {
                    access_token = GenerateJwt(user, roles),
                    role=roles
                });
            }
            return BadRequest("Email or password incorrect.");
        }
     
        private string GenerateJwt(User user, IList<string> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }

}
