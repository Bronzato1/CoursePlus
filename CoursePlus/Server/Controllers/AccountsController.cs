using System;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoursePlus.Server.Data;

namespace CoursePlus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private readonly UserManager<CustomUser> _userManager;

        public AccountsController(UserManager<CustomUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            try
            {
                var newUser = new CustomUser { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description);
                    return Ok(new RegisterResult { Successful = false, Errors = errors });
                }

                // Add all new users to the User role
                await _userManager.AddToRoleAsync(newUser, "User");

                // Add new users whose email starts with 'admin' to the Admin role
                if (newUser.Email.StartsWith("azur"))
                {
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                }

                return Ok(new RegisterResult { Successful = true });
            }
            catch
            {
                return Ok(new RegisterResult { Successful = false });
            }
            
        }
    }
}