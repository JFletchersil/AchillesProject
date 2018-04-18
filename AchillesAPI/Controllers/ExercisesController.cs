using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using AchillesAPI.Models.ViewModels;
using AchillesAPI.Contexts;
using AchillesAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using AchillesAPI.Helpers;
using AchillesAPI.Models.ViewModels.ErrorModels;

namespace AchillesAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private readonly AngularDbContext _context;
        private readonly ApplicationDbContext _appContext;
        private readonly AuthenticationHelper authenticationHelper;
        private readonly SessionExpiredViewModel sessionExpired;

        public ExercisesController(AngularDbContext context, ApplicationDbContext appContext)
        {
            _context = context;
            _appContext = appContext;
            authenticationHelper = new AuthenticationHelper();
            sessionExpired = new SessionExpiredViewModel()
            {
                ConsoleMessage = "User Session has Expired, Please Log into the Application",
                IsError = true,
                UserInformationMessage = "You are required to log back into the Application"
            };
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
        public IActionResult GetDailyExercises(Guid sessionID)
        {
            if (!authenticationHelper.VerifySession(sessionID, _appContext))
                return BadRequest(sessionExpired);

            var userID = authenticationHelper.DerriveUserIdFromSessionId(sessionID, _appContext);

            if (_context.UserExercises.Any(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date))
            {
                var currentExercisesInProgress = _context.UserExercises.Where(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date).ToList();

                var exerciseViewModels = new List<ExerciseViewModel>();

                foreach (var exercise in currentExercisesInProgress)
                {
                    var exerciseConfiguration = _context.Exercises.Include(x => x.ExerciseType).FirstOrDefault(x => x.ExerciseID == exercise.ExerciseStageID);
                    var completedExercise = JsonConvert.DeserializeObject<CompletedExerciseResults>(exercise.ResultsJSON);

                    var exeriseViewModel = new ExerciseViewModel()
                    {
                        Id = exercise.ExerciseStageID.ToString(),
                        Name = exerciseConfiguration.ExerciseName,
                        Reps = exerciseConfiguration.Reps,
                        Sets = exerciseConfiguration.Sets,
                        Time = exerciseConfiguration.Time,
                        CompletedResults = completedExercise,
                        VideoLink = exerciseConfiguration.VideoLink,
                        ExerciseType = (Models.ViewModels.ExerciseType)
                                        Enum.Parse(typeof(Models.ViewModels.ExerciseType), exerciseConfiguration.ExerciseType.ExerciseTypeEnum.ToString(), true)
                    };
                    exerciseViewModels.Add(exeriseViewModel);
                }
                return Ok(exerciseViewModels);
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
                    CompletedResults = new CompletedExerciseResults()
                    {
                        ExerciseId = x.ExerciseID,
                        CompletedReps = new List<double?>(),
                        CompletedSets = 0,
                        CompletedTimes = new List<double?>()
                    },
                    ExerciseType = (Models.ViewModels.ExerciseType)
                        Enum.Parse(typeof(Models.ViewModels.ExerciseType), x.ExerciseType.ExerciseTypeEnum.ToString(), true)
                }).ToList();

                var userExercises = viewModels.Select(x => new UserExercise()
                {
                    AuthUserID = userID,
                    DateTime = DateTime.Now,
                    ExerciseStageID = new Guid(x.Id),
                    UserExercisesID = Guid.NewGuid(),
                    ResultsJSON = JsonConvert.SerializeObject(x.CompletedResults)
                });

                _context.UserExercises.AddRange(userExercises.ToList());
                _context.SaveChanges();
                return Ok(viewModels);
            }
        }

        [HttpPost]
        [Route("SaveAllDailyExercises")]
        public IActionResult SaveAllDailyExercises(SaveMultipleExerciseProgressViewModel exerciseViewModel)
        {
            try
            {
                if (!TryValidateModel(exerciseViewModel))
                {
                    return BadRequest(new ErrorMessageViewModel()
                    {
                        ConsoleMessage = "Model Failed to validate",
                        IsError = true,
                        UserInformationMessage = "An Error has occurred, please refresh the application"
                    });
                }

                if (!authenticationHelper.VerifySession(exerciseViewModel.SessionId, _appContext))
                    return BadRequest(sessionExpired);
            }catch(Exception ex)
            {
                return BadRequest(new ErrorMessageViewModel()
                {
                    ConsoleMessage = ex.ToString(),
                    IsError = true,
                    UserInformationMessage = "An Error has occurred, please refresh the application"
                });
            }

            var currentExercisesInProgress = _context.UserExercises
                .Where(x => x.AuthUserID == exerciseViewModel.SessionId && x.DateTime.Date == DateTime.Now.Date).ToList();

            foreach (var exercise in exerciseViewModel.ResultsViewModel)
            {
                var currentSavedExercise = _context.UserExercises.FirstOrDefault(
                    x => x.AuthUserID == exerciseViewModel.SessionId
                    && x.DateTime.Date == DateTime.Now.Date
                    && exercise.ExerciseId == x.ExerciseStageID);

                if (currentSavedExercise == null)
                {
                    currentSavedExercise = new UserExercise()
                    {
                        AuthUserID = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _appContext),
                        ExerciseStageID = exercise.ExerciseId,
                        UserExercisesID = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        ResultsJSON = JsonConvert.SerializeObject(exercise)
                    };
                    _context.Add(currentSavedExercise);
                }
                else
                {
                    currentSavedExercise.DateTime = DateTime.Now;
                    currentSavedExercise.ResultsJSON = JsonConvert.SerializeObject(exercise);
                    _context.Update(currentSavedExercise);
                }
            }

            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageViewModel()
                {
                    ConsoleMessage = ex.ToString(),
                    IsError = true,
                    UserInformationMessage = "Changes were unable to be saved to the database, please record your progress, reload the application and try to save again"
                });
            }
        }

        [HttpPost]
        [Route("SaveSingleDailyExercises")]
        public IActionResult SaveSingleDailyExercise([FromBody]SaveSingleExerciseProgressViewModel exerciseViewModel)
        {
            try
            {
                if (!TryValidateModel(exerciseViewModel))
                {
                    return BadRequest(new ErrorMessageViewModel()
                    {
                        ConsoleMessage = "Model Failed to validate",
                        IsError = true,
                        UserInformationMessage = "An Error has occurred, please refresh the application"
                    });
                }

                if (!authenticationHelper.VerifySession(exerciseViewModel.SessionId, _appContext))
                    return BadRequest(sessionExpired);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageViewModel()
                {
                    ConsoleMessage = ex.ToString(),
                    IsError = true,
                    UserInformationMessage = "An Error has occurred, please refresh the application"
                });
            }

            var userId = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _appContext);

            var currentSavedExercise = _context.UserExercises.FirstOrDefault(
                x => x.AuthUserID == userId
                && x.DateTime.Date == DateTime.Now.Date 
                && exerciseViewModel.ResultViewModel.ExerciseId == x.ExerciseStageID);

            if(currentSavedExercise == null)
            {
                currentSavedExercise = new UserExercise()
                {
                    AuthUserID = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _appContext),
                    ExerciseStageID = exerciseViewModel.ResultViewModel.ExerciseId,
                    UserExercisesID = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    ResultsJSON = JsonConvert.SerializeObject(exerciseViewModel.ResultViewModel)
                };
                _context.Add(currentSavedExercise);
            }
            else
            {
                currentSavedExercise.DateTime = DateTime.Now;
                currentSavedExercise.ResultsJSON = JsonConvert.SerializeObject(exerciseViewModel.ResultViewModel);
                _context.Update(currentSavedExercise);
            }

            try
            {
                _context.SaveChanges();
                return Ok(new SuccessViewModel() { ConsoleMessage = "Has Saved", Success = true});
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageViewModel()
                {
                    ConsoleMessage = ex.ToString(),
                    IsError = true,
                    UserInformationMessage = "Changes were unable to be saved to the database, please record your progress, reload the application and try to save again"
                });
            }
        }
    }
}
