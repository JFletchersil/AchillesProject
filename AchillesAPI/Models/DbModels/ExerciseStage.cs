using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    [Table("ExerciseStages")]
    public class ExerciseStage
    {
        [Key]
        public Guid ExerciseStageID { get; set; }
        public Guid ExerciseID { get; set; }
        public Guid StageID { get; set; }
    }
}
