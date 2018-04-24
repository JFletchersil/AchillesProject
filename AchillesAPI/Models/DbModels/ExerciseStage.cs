using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The exercise stage model.
    /// This links the exercises to a specific stage, allowing you to make sure an exercise only
    /// appears within a stage.
    /// </summary>
    [Table("ExerciseStages")]
    public class ExerciseStage
    {
        /// <summary>
        /// Gets or sets the exercise stage identifier.
        /// </summary>
        /// <value>
        /// The exercise stage identifier.
        /// </value>
        [Key]
        public Guid ExerciseStageID { get; set; }
        /// <summary>
        /// Gets or sets the exercise identifier.
        /// </summary>
        /// <value>
        /// The exercise identifier.
        /// </value>
        public Guid ExerciseID { get; set; }
        /// <summary>
        /// Gets or sets the stage identifier.
        /// </summary>
        /// <value>
        /// The stage identifier.
        /// </value>
        public Guid StageID { get; set; }
    }
}
