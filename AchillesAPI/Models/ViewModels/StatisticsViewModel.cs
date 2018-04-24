using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The statistics view model
    /// </summary>
    public class StatisticsViewModel
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        /// 
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public string Results { get; set; }
        /// <summary>
        /// Gets or sets the exercise.
        /// </summary>
        /// <value>
        /// The exercise.
        /// </value>
        public string Exercise { get; set; }
        /// <summary>
        /// Gets or sets the reps.
        /// </summary>
        /// <value>
        /// The reps.
        /// </value>
        public double? Reps { get; set; }
        /// <summary>
        /// Gets or sets the sets.
        /// </summary>
        /// <value>
        /// The sets.
        /// </value>
        public double? Sets { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public double? Time { get; set; }
    }
}