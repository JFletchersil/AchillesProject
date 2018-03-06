using System.ComponentModel.DataAnnotations;

namespace AchillesAPI.Models.ViewModels
{
    public class ExerciseProgressViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public bool Completed { get; set; }
        public double Reps { get; set; }
        [Range(1, 3, ErrorMessage = "Outside of range")]
        public double Sets { get; set; }
        public double Time { get; set; }
    }
}
