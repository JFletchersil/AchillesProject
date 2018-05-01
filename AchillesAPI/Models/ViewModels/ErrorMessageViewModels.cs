using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The Error Message view model.
    /// </summary>
    public class ErrorMessageViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool IsError { get; set; } = true;
        /// <summary>
        /// Gets or sets the console message.
        /// </summary>
        /// <value>
        /// The console message.
        /// </value>
        public string ConsoleMessage { get; set; }
        /// <summary>
        /// Gets or sets the user information message.
        /// </summary>
        /// <value>
        /// The user information message.
        /// </value>
        public string UserInformationMessage { get; set; }
    }

    /// <summary>
    /// The session expired view model.
    /// </summary>
    /// <seealso cref="ErrorMessageViewModel" />
    public class SessionExpiredViewModel : ErrorMessageViewModel
    {
        /// <summary>
        /// Gets a value indicating whether this instance has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has expired; otherwise, <c>false</c>.
        /// </value>
        public bool HasExpired { get { return true; } }
    }
}
