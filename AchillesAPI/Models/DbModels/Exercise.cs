using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The exercise model.
    /// </summary>
    [Table("Exercises")]
    public class Exercise
    {
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        [Key]
        public Guid ExerciseID { get; set; }
        /// <summary>
        /// Gets or sets the name of the exercise.
        /// </summary>
        /// <value>
        /// The name of the exercise.
        /// </value>
        public string ExerciseName { get; set; }
        /// <summary>
        /// Gets or sets the video link.
        /// </summary>
        /// <value>
        /// The video link.
        /// </value>
        public string VideoLink { get; set; }
        /// <summary>
        /// Gets or sets the exercise type identifier.
        /// </summary>
        /// <value>
        /// The exercise type identifier.
        /// </value>
        public Guid ExerciseTypeID { get; set; }
        /// <summary>
        /// Gets or sets the type of the exercise.
        /// </summary>
        /// <value>
        /// The type of the exercise.
        /// </value>
        [ForeignKey("ExerciseTypeID")]
        [Required]
        public virtual ExerciseType ExerciseType { get; set; }
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
        /// Gets or sets the previous sub stage exercise identifier.
        /// </summary>
        /// <value>
        /// The previous sub stage exercise identifier.
        /// </value>
        public string PreviousSubStageExerciseID { get; set; }
        /// <summary>
        /// Gets or sets the future sub stage exercise identifier.
        /// </summary>
        /// <value>
        /// The future sub stage exercise identifier.
        /// </value>
        public string FutureSubStageExerciseID { get; set; }
    }
}
