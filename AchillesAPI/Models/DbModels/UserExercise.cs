using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The user exercise model.
    /// Used to model the exercises that a user has performed.
    /// </summary>
    [Table("UserExercises")]
    public class UserExercise
    {
        /// <summary>
        /// Gets or sets the user exercises identifier.
        /// </summary>
        /// <value>
        /// The user exercises identifier.
        /// </value>
        [Key]
        public Guid UserExercisesID { get; set; }
        /// <summary>
        /// Gets or sets the exercise stage identifier.
        /// </summary>
        /// <value>
        /// The exercise stage identifier.
        /// </value>
        public Guid ExerciseStageID { get; set; }
        /// <summary>
        /// Gets or sets the authentication user identifier.
        /// </summary>
        /// <value>
        /// The authentication user identifier.
        /// </value>
        public Guid AuthUserID { get; set; }
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Gets or sets the results json.
        /// </summary>
        /// <value>
        /// The results json.
        /// </value>
        public string ResultsJSON { get; set; }
    }
}
