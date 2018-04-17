using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    [Table("UserExercises")]
    public class UserExercise
    {
        [Key]
        public Guid UserExercisesID { get; set; }
        public Guid ExerciseStageID { get; set; }
        public Guid AuthUserID { get; set; }
        public DateTime DateTime { get; set; }
        public string ResultsJSON { get; set; }
    }
}
