using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The exercise type model.
    /// </summary>
    [Table("ExerciseTypes")]
    public class ExerciseType
    {
        /// <summary>
        /// Gets or sets the exercise type identifier.
        /// </summary>
        /// <value>
        /// The exercise type identifier.
        /// </value>
        [Key]
        [Required]
        public Guid ExerciseTypeID { get; set; }
        /// <summary>
        /// Gets or sets the exercise type enum.
        /// </summary>
        /// <value>
        /// The exercise type enum.
        /// </value>
        public ExerciseTypes ExerciseTypeEnum { get; set; }
        /// <summary>
        /// Gets or sets the name of the exercise type.
        /// </summary>
        /// <value>
        /// The name of the exercise type.
        /// </value>
        public string ExerciseTypeName { get; set; }
        /// <summary>
        /// Gets or sets the exercises.
        /// </summary>
        /// <value>
        /// The exercises.
        /// </value>
        public ICollection<Exercise> Exercises { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ExerciseTypes
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
