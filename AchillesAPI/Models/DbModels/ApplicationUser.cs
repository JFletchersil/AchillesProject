using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AchillesAPI.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    /// <summary>
    /// The model used to interface with the Identity users within the application.
    /// Includes an additional property to track which stage the user is currently in.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser" />
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the user level.
        /// </summary>
        /// <value>
        /// The user level.
        /// </value>
        public int UserLevel { get; set; }
    }
}
