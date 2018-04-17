using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The model used to log a user in, including both a user name/email and password
    /// </summary>
    /// <remarks>
    /// This is actually used inside the project, and as such should not be altered.
    /// It is used inside the AccountController.
    /// </remarks>
    /// <seealso cref="Controllers.AccountController" />
    public class LoginViewModel
    {
        /// <summary>
        /// The email that a user is logging in with
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// The password that the user is using to log into the account
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// If the user should remain logged in at the current browser/location
        /// </summary>
        /// <value>
        ///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
