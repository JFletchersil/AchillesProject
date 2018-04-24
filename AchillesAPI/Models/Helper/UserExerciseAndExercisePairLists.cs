using AchillesAPI.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.Helper
{
    /// <summary>
    /// The User Exercise and Exercise pair, this is a pair of exercises and
    /// user exercises that logically belong together.
    /// </summary>
    public class UserExerciseAndExercisePairLists
    {
        /// <summary>
        /// Gets or sets the previous user exercises.
        /// </summary>
        /// <value>
        /// The previous user exercises.
        /// </value>
        public List<UserExercise> PreviousUserExercises { get; set; }
        /// <summary>
        /// Gets or sets the previous standard exercises.
        /// </summary>
        /// <value>
        /// The previous standard exercises.
        /// </value>
        public List<Exercise> PreviousStandardExercises { get; set; }
    }
}
