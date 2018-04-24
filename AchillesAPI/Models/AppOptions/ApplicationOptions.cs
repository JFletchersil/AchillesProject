using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.AppOptions
{
    /// <summary>
    /// The options that can be placed inside the appSettings.json
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// Gets or sets the daily exercises.
        /// </summary>
        /// <value>
        /// The daily exercises.
        /// </value>
        public int DailyExercises { get; set; }
    }
}
