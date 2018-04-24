using System.ComponentModel.DataAnnotations;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The exercise progress view model.
    /// </summary>
    public class ExerciseProgressViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExerciseProgressViewModel"/> is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if completed; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool Completed { get; set; }
        /// <summary>
        /// Gets or sets the reps.
        /// </summary>
        /// <value>
        /// The reps.
        /// </value>
        public double Reps { get; set; }
        /// <summary>
        /// Gets or sets the sets.
        /// </summary>
        /// <value>
        /// The sets.
        /// </value>
        public double Sets { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public double Time { get; set; }
    }
}
