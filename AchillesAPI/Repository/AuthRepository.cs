using AchillesAPI.Contexts;
using AchillesAPI.Models.ViewModels;
using AchilliesLogin.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Repository
{
    /// <summary>
    /// A collection of repositories that allow the user to work with the various databases that are
    /// associated with the project
    /// </summary>
    /// <remarks>
    /// At present, there is only one repository that is used.
    /// This repository is the Auth Repository
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// A repository for handling Authentication and User Management
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class AuthRepository : IDisposable
    {
        /// <summary>
        /// The Authorization Context that the Repository uses
        /// </summary>
        private ApplicationDbContext _ctx;

        /// <summary>
        /// The user manager
        /// </summary>
        private UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// The role store
        /// </summary>
        private RoleStore<IdentityRole> _roleStore;
        /// <summary>
        /// The role manager
        /// </summary>
        private RoleManager<IdentityRole> _roleMngr;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRepository"/> class.
        /// </summary>
        /// <seealso cref="Controllers.AccountController"/>
        public AuthRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _ctx = dbContext;
            _userManager = userManager;
            _roleStore = new RoleStore<IdentityRole>(_ctx);
            _roleMngr = roleManager;
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <remarks>
        /// This allows a user to create their new user account with basic permissions.
        /// It also has an extension to allow an administrator to create a new account with 
        /// administration permissions.
        /// </remarks>
        /// <param name="userModel">The user model.</param>
        /// <returns>If the creation of the user was successful or not</returns>
        /// <seealso cref="Controllers.AccountController"/>
        public async Task<IdentityResult> RegisterUser(RegisterViewModel userModel)
        {
            // Gives the new user an identifier
            var currGuid = Guid.NewGuid().ToString();
            var user = new ApplicationUser
            {
                Id = currGuid,
                UserName = userModel.UserName,
                // Gives the user the correct roles, by default this is done in such a fashion
                // as to make user the default role but an administrator can alter this.
                //Roles =
                //{
                //    new IdentityUserRole()
                //    {
                //        RoleId = (userModel.IsAdministrator) ? WebConfigurationManager.AppSettings["AdministratorRole"] : WebConfigurationManager.AppSettings["UserRole"],
                //        UserId = currGuid
                //    }
                //},
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        /// <summary>
        /// Finds a given user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A single user who matches the user name and password</returns>
        /// <seealso cref="Controllers.AccountController"/>
        public async Task<IdentityUser> FindUser(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            return user;
        }

        /// <summary>
        /// Finds the user by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Returns a user who matches a given unique identifier</returns>
        public IdentityUser FindUserByGuid(string guid)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == guid);
            return user;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users currently within the database</returns>
        /// <seealso cref="Controllers.AccountController"/>
        public List<ApplicationUser> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}