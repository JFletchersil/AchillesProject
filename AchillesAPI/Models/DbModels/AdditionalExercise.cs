using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The additional exercises model.
    /// </summary>
    [Table("AdditionalExercises")]
    public class AdditionalExercise
    {
        /// <summary>
        /// Gets or sets the relation identifier.
        /// </summary>
        /// <value>
        /// The relation identifier.
        /// </value>
        [Key]
        public Guid RelationID { get; set; }
        /// <summary>
        /// Gets or sets the stage.
        /// </summary>
        /// <value>
        /// The stage.
        /// </value>
        [ForeignKey("StageID")]
        [Required]
        public virtual Stage Stage { get; set; }
        /// <summary>
        /// Gets or sets the name of the exercise.
        /// </summary>
        /// <value>
        /// The name of the exercise.
        /// </value>
        public string ExerciseName { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
    }
}
