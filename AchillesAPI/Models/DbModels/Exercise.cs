using System;
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
        public ExerciseType ExerciseType { get; set; }
        // Three below need to be changed in DB
        public double Reps { get; set; }
        public double Sets { get; set; }
        public double Time { get; set; }
    }
}
