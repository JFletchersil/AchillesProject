using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utility;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using AchillesAPI.Models.ViewModels;
using AchillesAPI.Contexts;
using AchillesAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AchillesAPI.Controllers
{
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private readonly AngularDbContext _context;

        public ExercisesController(AngularDbContext context)
        {
            _context = context;
        }

        // GET api/exercises/additional
        [Route("additional/{stage}")]
        public string getTest(int stage)
        {
            var availableExercises = new List<String>();

            switch (stage)
            {
                case 1:
                    availableExercises.Add("Running");
                    availableExercises.Add("Swimming");
                    availableExercises.Add("Cycling");
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            return JsonConvert.SerializeObject(availableExercises);
        }

        [HttpGet]
        [Route("GetDailyExercises")]
        public List<ExerciseViewModel> GetDailyExercises()
        {
            var exercises = _context.Exercises.Include(x => x.ExerciseType).ToList();
            var viewModels = exercises.Select(x => new ExerciseViewModel
            {
                Id = x.ExerciseID.ToString(),
                Name = x.ExerciseName,
                Reps = x.Reps,
                Sets = x.Sets,
                Time = x.Time,
                VideoLink = x.VideoLink,
                ExerciseType = (Models.ViewModels.ExerciseType)
                    Enum.Parse(typeof(Models.ViewModels.ExerciseType), x.ExerciseType.ExerciseTypeEnum.ToString(), true)
            }).ToList();

            var zip = exercises.Zip(viewModels, (x, y) => new { Exercise = x, ViewModel = y });

            //foreach (var a in zip)
            //{
            //    a.ViewModel.ExerciseType = (Models.ViewModels.ExerciseType)
            //        Enum.Parse(typeof(Models.ViewModels.ExerciseType), a.Exercise.ExerciseType.ExerciseTypeEnum.ToString(), true);
            //}

            return zip.Select(x => x.ViewModel).ToList();
        }
    }
}