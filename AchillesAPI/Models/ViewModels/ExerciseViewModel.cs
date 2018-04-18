using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    public class SaveSingleExerciseProgressViewModel
    {
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public CompletedExerciseResults ResultViewModel { get; set; }
    }

    public class SaveMultipleExerciseProgressViewModel
    {
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public List<CompletedExerciseResults> ResultsViewModel { get; set; }
    }

    public class ExerciseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string VideoLink { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public double? Reps { get; set; }
        public double? Sets { get; set; }
        public double? Time { get; set; }
        public CompletedExerciseResults CompletedResults { get; set; }
    }

    public class CompletedExerciseResults
    {
        [Required]
        public Guid ExerciseId { get; set; }
        public List<double?> CompletedReps { get; set; }
        public double? CompletedSets { get; set; }
        public List<double?> CompletedTimes { get; set; }
    }

    public enum ExerciseType
    {
        RepsSets = 0,
        Timed = 1
    }
}
