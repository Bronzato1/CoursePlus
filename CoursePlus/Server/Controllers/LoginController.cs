using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CoursePlus.Server.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CoursePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        public LoginController(IConfiguration configuration, SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

                if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username or password are invalid." });

                var user = await _signInManager.UserManager.FindByEmailAsync(login.Email);
                var roles = await _signInManager.UserManager.GetRolesAsync(user);
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                
                var claims = claimsPrincipal.Claims.ToList();

                //claims.Add(new Claim(ClaimTypes.Name, login.Email));

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: expiry,
                    signingCredentials: creds
                );

                return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch
            {
                return Ok(new LoginResult { Successful = false });
            }
            
        }
    }
}