using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    public class ExerciseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string VideoLink { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public double? Reps { get; set; }
        public double? Sets { get; set; }
        public double? Time { get; set; }
    }

    public enum ExerciseType
    {
        RepsSets = 0,
        Timed = 1
    }
}
