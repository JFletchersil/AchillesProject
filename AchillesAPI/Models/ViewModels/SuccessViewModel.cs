using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The success view model
    /// </summary>
    public class SuccessViewModel
    {
        /// <summary>
        /// Gets or sets the console message.
        /// </summary>
        /// <value>
        /// The console message.
        /// </value>
        public string ConsoleMessage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SuccessViewModel"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }
    }
}
