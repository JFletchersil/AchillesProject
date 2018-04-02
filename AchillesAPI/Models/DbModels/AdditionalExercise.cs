using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    [Table("AdditionalExercises")]
    public class AdditionalExercise
    {
        [Key]
        public Guid RelationID { get; set; }
        [ForeignKey("StageID")]
        [Required]
        public virtual Stage Stage { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
    }
}
