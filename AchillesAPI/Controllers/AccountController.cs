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
    /// <summary>
    /// A collection of controllers designed to allow the front end to access API functionality.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// The controller for the account functionality within the program.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// The sign in manager
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
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
        /// <returns>
        /// A 200 or 500 response depending on if the login action was successful or not
        /// </returns>
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

        /// <summary>
        /// Validates the result.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>An <see cref="IActionResult"/> containing either a valid session GUID or the exception that occured.</returns>
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

        /// <summary>
        /// Determines whether [is super admin] [the specified session identifier].
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>An <see cref="IActionResult"/> containing either a bool or a 400 bad request object.</returns>
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

        /// <summary>
        /// Registers the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An <see cref="IActionResult"/> that contains a bool or a bad request containing either a collection of errors or just a bad request.</returns>
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

        /// <summary>
        /// Edits the user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>An <see cref="IActionResult"/>, this will be either an Ok result containing the number of records changed, or a BadRequest containing the same.</returns>
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

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>An <see cref="IActionResult"/> containing all current users within the system, or a BadRequest Result.</returns>
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

        /// <summary>
        /// Determines whether [is super admin private] [the specified session identifier].
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is super admin private] [the specified session identifier]; otherwise, <c>false</c>.
        /// </returns>
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