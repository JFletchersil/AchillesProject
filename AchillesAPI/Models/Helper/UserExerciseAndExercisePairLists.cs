using AchillesAPI.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.Helper
{
    public class UserExerciseAndExercisePairLists
    {
        public List<UserExercise> PreviousUserExercises { get; set; }
        public List<Exercise> PreviousStandardExercises { get; set; }
    }
}
