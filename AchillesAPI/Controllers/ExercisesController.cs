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
using Newtonsoft.Json.Linq;

namespace AchillesAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private readonly AngularDbContext _context;
        private readonly ApplicationDbContext _appContext;

        public ExercisesController(AngularDbContext context, ApplicationDbContext appContext)
        {
            _context = context;
            _appContext = appContext;
        }

        // GET api/exercises/additional
        [Route("additional/{stage}")]
        public List<AdditionalExerciseViewModel> getAdditionalExercises(int stage)
        {
            //Get all stages available to the user
            var stages = (from s in _context.Stages
                          where s.StageNumber <= stage
                          select s.StageID).ToList();

            //Use these to select the correct entries and produce relevant VMs
            return _context.AdditionalExercises
                .Where(e => stages.Contains(e.Stage.StageID))
                .Select(e => new AdditionalExerciseViewModel
                {
                    Exercise = e.ExerciseName,
                    Description = e.Description
                }).ToList();
        }

        [HttpGet]
        [Route("GetDailyExercises")]
        public List<ExerciseViewModel> GetDailyExercises(string sessionID)
        {
            var userID = _appContext.UserSessions.FirstOrDefault(x => x.SessionId == new Guid(sessionID)).UserId;
            if (_context.UserExercises.Any(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date))
            {
                var currentExercisesInProgress = _context.UserExercises.Where(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date).ToList();

                var exerciseViewModels = new List<ExerciseViewModel>();

                foreach (var exercise in currentExercisesInProgress)
                {
                    var exerciseConfiguration = _context.Exercises.ToList().FirstOrDefault(x => x.ExerciseID == exercise.ExerciseStageID);
                    var resultsJSON = JsonConvert.DeserializeObject<JObject>(exercise.ResultsJSON);

                    var cReps = resultsJSON["Reps"]?.ToObject<List<double?>>();
                    var cSets = resultsJSON["Sets"]?.ToObject<double?>();
                    var cTime = resultsJSON["Time"]?.ToObject<List<double?>>();

                    if (cReps == null)
                    {
                        cReps = new List<double?>();
                    }

                    if (cSets == null)
                    {
                        cSets = -1;
                    }

                    if (cTime == null)
                    {
                        cTime = new List<double?>();
                    }

                    var exeriseViewModel = new ExerciseViewModel()
                    {
                        Id = exercise.ExerciseStageID.ToString(),
                        Name = exerciseConfiguration.ExerciseName,
                        Reps = exerciseConfiguration.Reps,
                        Sets = exerciseConfiguration.Sets,
                        Time = exerciseConfiguration.Time,
                        CompletedReps = cReps,
                        CompletedSets = cSets,
                        CompletedTimes = cTime,
                        VideoLink = exerciseConfiguration.VideoLink,
                        ExerciseType = (Models.ViewModels.ExerciseType)
                                        Enum.Parse(typeof(Models.ViewModels.ExerciseType), exerciseConfiguration.ExerciseType.ExerciseTypeEnum.ToString(), true)
                    };
                    exerciseViewModels.Add(exeriseViewModel);
                }
                return exerciseViewModels;
            }
            else
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

                var userExercises = viewModels.Select(x => new UserExercise()
                {
                    AuthUserID = userID,
                    DateTime = DateTime.Now,
                    ExerciseStageID = new Guid(x.Id),
                    UserExercisesID = Guid.NewGuid()
                });

                _context.UserExercises.AddRange(userExercises.ToList());
                // var zip = exercises.Zip(viewModels, (x, y) => new { Exercise = x, ViewModel = y });
                // zip.Select(x => x.ViewModel).ToList();
                return viewModels;
            }
        }

        [HttpPost]
        [Route("SaveDailyExercises")]
        public IActionResult SaveDailyExercises(List<ExerciseViewModel> exerciseViewModel)
        {
            return Ok();
        }
    }
}