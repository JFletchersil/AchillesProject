using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The user session model.
    /// This is used to ensure that the user cannot remain logged in forever.
    /// By default, the user will only remain logged in for 30 days.
    /// </summary>
    [Table("AspNetSessionTable")]
    public class UserSession
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [Key]
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the expires when.
        /// </summary>
        /// <value>
        /// The expires when.
        /// </value>
        public DateTime ExpiresWhen { get; set; }
    }
}
