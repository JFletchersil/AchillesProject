using System;
using System.Linq;
using System.Threading.Tasks;
using AchillesAPI.Contexts;
using AchillesAPI.Helpers;
using AchillesAPI.Models;
using AchillesAPI.Models.DbModels;
using AchillesAPI.Models.ViewModels;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
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
                    var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
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
        public IActionResult ValidateResult(Guid sessionId)
        {
            try
            {
                var helper = new AuthenticationHelper();
                return Ok(helper.VerifySession(sessionId, _context));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("IsAdmin")]
        public IActionResult IsSuperAdmin(Guid sessionId)
        {
            var helper = new AuthenticationHelper();
            if (helper.VerifySession(sessionId, _context))
            {
                return Ok(IsSuperAdminPrivate(sessionId));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel input)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = input.Email, Email = input.Email, UserLevel = 1 };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    var addToRole = await _userManager.AddToRoleAsync(user, "User");
                    return Ok(addToRole.Succeeded);
                }
                return BadRequest(result.Errors.ToList());
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditUser([FromBody]ValidEditUserViewModel userModel)
        {
            var helper = new AuthenticationHelper();
            if (helper.VerifySession(userModel.SessionId, _context) && IsSuperAdminPrivate(userModel.SessionId))
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userModel.UserModel.Id.ToString());
                if (user != null)
                {
                    user.Email = userModel.UserModel.Email;
                    user.UserName = userModel.UserModel.UserName;
                    user.UserLevel = userModel.UserModel.UserLevel;
                    try
                    {
                        _context.Update(user);
                        _context.SaveChanges();
                        return Ok(1);
                    }
                    catch (Exception)
                    {
                        return BadRequest(0);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers(Guid sessionId)
        {
            var helper = new AuthenticationHelper();
            if (helper.VerifySession(sessionId, _context) && IsSuperAdminPrivate(sessionId))
            {
                var users = _userManager.GetUsersInRoleAsync("User").Result.Select(x => new UserViewModel()
                {
                    Email = x.Email,
                    Id = new Guid(x.Id),
                    UserLevel = x.UserLevel,
                    UserName = x.UserName
                }).ToList();
                return Ok(users);
            }
            else
            {
                return BadRequest();
            }
        }

        private bool IsSuperAdminPrivate(Guid sessionId)
        {
            var helper = new AuthenticationHelper();
            var userId = helper.DerriveUserIdFromSessionId(sessionId, _context);
            var usersInRole = _userManager.GetUsersInRoleAsync("SuperAdmin").Result;
            var isSuperAdmin = usersInRole.Any(x => x.Id == userId.ToString());
            return isSuperAdmin;
        }
    }
}