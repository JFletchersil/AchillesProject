using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillesAPI.Models.DbModels
{
    [Table("Exercises")]
    public class Exercise
    {
        [Key]
        public Guid ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public string VideoLink { get; set; }
        public Guid ExerciseTypeID { get; set; }
        [ForeignKey("ExerciseTypeID")]
        [Required]
        public virtual ExerciseType ExerciseType { get; set; }
        public double? Reps { get; set; }
        public double? Sets { get; set; }
        public double? Time { get; set; }
        public string PreviousSubStageExerciseID { get; set; }
        public string FutureSubStageExerciseID { get; set; }
    }
}
