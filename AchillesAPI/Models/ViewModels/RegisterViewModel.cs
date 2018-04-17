using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// This is the model used to register new users into the current account system
    /// </summary>
    /// <remarks>
    /// In future, this would be replaced with the various Auth0 models, but until then, this remains
    /// the registration model.
    /// This model is used within the program, at Controllers.AccountController and Repository.AuthRepository
    /// </remarks>
    /// <seealso cref="Controllers.AccountController" />
    /// <seealso cref="Repository.AuthRepository" />
    /// <seealso cref="Controllers.Auth0Controller" />
    public class RegisterViewModel
    {
        /// <summary>
        /// The email with which the user will be using as both user name and email address for the account system
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// The starting password that the user will use to log into the system
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// A copy of the previous property, this is the password that the user will use to log into the system
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        /// <remarks>
        /// This must match the Password property, otherwise it will be rejected out of hand, this is to
        /// prevent typos inside the password entry
        /// </remarks>
        /// <seealso cref="Password" />
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Sets the initial configuration for if the user is an Administration user
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is administrator; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// By default, the user is assumed to not be an Administration level user.
        /// </remarks>
        [Display(Name = "Is Administration")]
        public bool IsAdministrator { get; set; } = false;

        /// <summary>
        /// The user name that the user will have within the system, this is also used in conjunction with email
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        /// <seealso cref="Email" />
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
    }
}
