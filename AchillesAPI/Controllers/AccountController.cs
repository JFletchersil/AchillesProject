using System;
using System.Threading.Tasks;
using AchillesAPI.Contexts;
using AchillesAPI.Helpers;
using AchillesAPI.Models;
using AchillesAPI.Models.DbModels;
using AchillesAPI.Models.ViewModels;
using AchillesAPI.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AchillesAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthRepository _repo;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _repo = new AuthRepository(context, userManager, roleManager);
            _signInManager = signInManager;
        }

        /// <summary>
        /// Logins the specified login view model.
        /// </summary>
        /// <param name="loginViewModel">The login view model.</param>
        /// <returns>A 200 or 500 response depending on if the login action was successful or not</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);

                if (result != null && result.Succeeded)
                {
                    var user = await _repo.FindUser(loginViewModel.Email);
                    var sessionState = new UserSession()
                    {
                        UserId = new Guid(user.Id),
                        SessionId = Guid.NewGuid(),
                        ExpiresWhen = DateTime.Now.AddDays(30)
                    };

                    _context.UserSessions.Add(sessionState);
                    _context.SaveChanges();

                    return Ok(sessionState.SessionId);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("ValidateSession")]
        public IActionResult ValidateResult(string sessionId)
        {
            try
            {
                var helper = new AuthenticationHelper();
                return Ok(helper.VerifySession(new Guid(sessionId), _context));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}