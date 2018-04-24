using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    /// <summary>
    /// The save single exercise progress view model.
    /// </summary>
    public class SaveSingleExerciseProgressViewModel
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [Required]
        public Guid SessionId { get; set; }
        /// <summary>
        /// Gets or sets the result view model.
        /// </summary>
        /// <value>
        /// The result view model.
        /// </value>
        [Required]
        public CompletedExerciseResults ResultViewModel { get; set; }
    }

    /// <summary>
    /// The save multiple exercise progress view model.
    /// </summary>
    public class SaveMultipleExerciseProgressViewModel
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [Required]
        public Guid SessionId { get; set; }
        /// <summary>
        /// Gets or sets the results view model.
        /// </summary>
        /// <value>
        /// The results view model.
        /// </value>
        [Required]
        public List<CompletedExerciseResults> ResultsViewModel { get; set; }
    }

    /// <summary>
    /// The exercise view model.
    /// </summary>
    public class ExerciseViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the video link.
        /// </summary>
        /// <value>
        /// The video link.
        /// </value>
        public string VideoLink { get; set; }
        /// <summary>
        /// Gets or sets the type of the exercise.
        /// </summary>
        /// <value>
        /// The type of the exercise.
        /// </value>
        public ExerciseType ExerciseType { get; set; }
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
        /// <summary>
        /// Gets or sets the completed results.
        /// </summary>
        /// <value>
        /// The completed results.
        /// </value>
        public CompletedExerciseResults CompletedResults { get; set; }
    }

    /// <summary>
    /// The completed exercise results view model.
    /// </summary>
    public class CompletedExerciseResults
    {
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        [Required]
        public Guid ExerciseId { get; set; }
        /// <summary>
        /// Gets or sets the completed reps.
        /// </summary>
        /// <value>
        /// The completed reps.
        /// </value>
        public List<double?> CompletedReps { get; set; }
        /// <summary>
        /// Gets or sets the completed times.
        /// </summary>
        /// <value>
        /// The completed times.
        /// </value>
        public List<double?> CompletedTimes { get; set; }
    }

    /// <summary>
    /// The exercise type enum
    /// </summary>
    public enum ExerciseType
    {
        /// <summary>
        /// The reps sets
        /// </summary>
        RepsSets = 0,
        /// <summary>
        /// The timed
        /// </summary>
        Timed = 1
    }
}
