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
using AchillesAPI.Helpers;
using AchillesAPI.Models.ViewModels.ErrorModels;
using Microsoft.Extensions.Options;
using AchillesAPI.Models.AppOptions;
using AchillesAPI.Models.Helper;

namespace AchillesAPI.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private readonly AngularDbContext _context;
        private readonly ApplicationDbContext _authContext;
        private readonly AuthenticationHelper authenticationHelper;
        private readonly SessionExpiredViewModel sessionExpired;
        private readonly ApplicationOptions _appOptions;

        public ExercisesController(AngularDbContext context, ApplicationDbContext appContext,
            IOptions<ApplicationOptions> optionsAccessor)
        {
            _context = context;
            _authContext = appContext;
            _appOptions = optionsAccessor.Value;
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
            if (!authenticationHelper.VerifySession(sessionID, _authContext))
                return BadRequest(sessionExpired);

            var userID = authenticationHelper.DerriveUserIdFromSessionId(sessionID, _authContext);
            var isOldUser = _context.UserExercises.Any(x => x.AuthUserID == userID);
            if (_context.UserExercises.Any(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date))
            {
                var currentExercisesInProgress = _context.UserExercises.Where(x => x.AuthUserID == userID && x.DateTime.Date == DateTime.Now.Date).ToList();

                var exerciseViewModels = new List<ExerciseViewModel>();

                foreach (var exercise in currentExercisesInProgress)
                {
                    var exerciseConfiguration = _context.Exercises.Include(x => x.ExerciseType).FirstOrDefault(x => x.ExerciseID == exercise.ExerciseStageID);
                    var completedExercise = JsonConvert.DeserializeObject<CompletedExerciseResults>(exercise.ResultsJSON);
                    exerciseViewModels.Add(GenerateNewExerciseViewModel(exerciseConfiguration, completedExercise));
                }
                return Ok(exerciseViewModels);
            }
            else
            {
                var exercises = new List<Exercise>();
                if (isOldUser)
                {
                    var exercisesWithinStage = GetExercisesByUserStage(userID);
                    var exercisesFromLastExerciseSession =
                        GetAllExercisesWithinCorrectStageFromLastExerciseSession(userID, exercisesWithinStage);

                    foreach (var userExercise in exercisesFromLastExerciseSession.PreviousUserExercises)
                    {
                        var associatedPreviousExercise = exercisesFromLastExerciseSession.PreviousStandardExercises
                            .FirstOrDefault(x => x.ExerciseID == userExercise.ExerciseStageID);
                        var hasPreviousExercise = !string.IsNullOrWhiteSpace(associatedPreviousExercise.
                            PreviousSubStageExerciseID);
                        var hasFutureExercise = !string.IsNullOrWhiteSpace(associatedPreviousExercise.
                            FutureSubStageExerciseID);

                        var exerciseProgress = JsonConvert
                            .DeserializeObject<CompletedExerciseResults>(userExercise.ResultsJSON);
                        switch (associatedPreviousExercise.ExerciseType.ExerciseTypeEnum)
                        {
                            case ExerciseTypes.Timed:
                                exercises.Add(EvaluateIfUserMovesAheadOrBehindExercises(associatedPreviousExercise,
                                    userExercise, hasFutureExercise, hasPreviousExercise, exerciseProgress.CompletedTimes,
                                    associatedPreviousExercise.Time));
                                break;
                            case ExerciseTypes.RepsSets:
                                exercises.Add(EvaluateIfUserMovesAheadOrBehindExercises(associatedPreviousExercise,
                                    userExercise, hasFutureExercise, hasPreviousExercise, exerciseProgress.CompletedReps,
                                    associatedPreviousExercise.Reps));
                                break;
                            default:
                                return BadRequest();
                        }
                    }

                    var missingExercises = _appOptions.DailyExercises - exercises.Count();
                    if (missingExercises != 0)
                    {
                        var idOfExericses = exercises.Select(x => x.ExerciseID);
                        var potentialExercises = _context.Exercises.Where(x => !idOfExericses.Any(y => y == x.ExerciseID)
                        && (string.IsNullOrEmpty(x.PreviousSubStageExerciseID)));
                        exercises.AddRange(potentialExercises.Take(missingExercises));
                    }
                }
                else
                {
                    exercises = GetExercisesByUserStage(userID, false);
                }

                var viewModels = exercises.Select(x => GenerateNewExerciseViewModel(x));

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

                if (!authenticationHelper.VerifySession(exerciseViewModel.SessionId, _authContext))
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
                        AuthUserID = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _authContext),
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

                if (!authenticationHelper.VerifySession(exerciseViewModel.SessionId, _authContext))
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

            var userId = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _authContext);

            var currentSavedExercise = _context.UserExercises.FirstOrDefault(
                x => x.AuthUserID == userId
                && x.DateTime.Date == DateTime.Now.Date
                && exerciseViewModel.ResultViewModel.ExerciseId == x.ExerciseStageID);

            if (currentSavedExercise == null)
            {
                currentSavedExercise = new UserExercise()
                {
                    AuthUserID = authenticationHelper.DerriveUserIdFromSessionId(exerciseViewModel.SessionId, _authContext),
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
                return Ok(new SuccessViewModel() { ConsoleMessage = "Has Saved", Success = true });
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

        private List<Exercise> GetExercisesByUserStage(Guid userID, bool allExercises = true)
        {
            // Gets the current user level
            var userLevel = _authContext.Users.FirstOrDefault(x => x.Id == userID.ToString()).UserLevel;
            // Gets the stage ID for the users current level
            var exerciseLevelId = _context.Stages.FirstOrDefault(x => x.StageNumber == userLevel).StageID;
            // Gets all of the exercises, from the exercise stage table, that have the current stage ID
            var exercisesOfLevel = _context.ExerciseStages.Where(x => x.StageID == exerciseLevelId);
            // Get all of the exercises within the database at the users level, 
            // including the exercise type in the object
            // Depending on need, either include all exercises in order to filter based off previous exercises
            // performed, or only return exercises that are at the start of a chain of exercises
            if (allExercises)
            {
                return _context.Exercises.Include(x => x.ExerciseType)
                    .Where(y => exercisesOfLevel.Any(x => x.ExerciseID == y.ExerciseID)).ToList();
            }
            else
            {
                return _context.Exercises.Include(x => x.ExerciseType)
                    .Where(y => exercisesOfLevel.Any(x => x.ExerciseID == y.ExerciseID) 
                    && y.PreviousSubStageExerciseID == null).ToList();
            }
        }

        private UserExerciseAndExercisePairLists GetAllExercisesWithinCorrectStageFromLastExerciseSession(Guid userID,
            List<Exercise> exercisesWithinStage)
        {
            var previousDate = _context.UserExercises.Where(x => x.AuthUserID == userID && x.DateTime.Date != DateTime.Now.Date)
                .OrderByDescending(x => x.DateTime).FirstOrDefault().DateTime;
            var previousUserExercises = _context.UserExercises.Where(x => x.DateTime.Date == previousDate.Date).ToList();
            var previousExercisesFromExerciseTable = exercisesWithinStage
                .Where(y => previousUserExercises.Any(x => x.ExerciseStageID == y.ExerciseID)).ToList();
            return new UserExerciseAndExercisePairLists()
            {
                PreviousStandardExercises = previousExercisesFromExerciseTable,
                PreviousUserExercises = previousUserExercises
            };
        }

        private Exercise EvaluateIfUserMovesAheadOrBehindExercises(Exercise associatedPreviousExercise,
            UserExercise exerciseProgress, bool hasFutureExercise, bool hasPreviousExercise,
            List<double?> completedItems, double? completedLimit)
        {
            if (!completedLimit.HasValue || !completedItems.All(x => x.HasValue))
            {
                throw new ArgumentNullException("CompletedLimit or CompletedItems are null");
            }

            try
            {
                var hasCompleted = completedItems.All(x => x.Value == completedLimit) 
                    && completedItems.Count() == associatedPreviousExercise.Sets;

                if (hasCompleted)
                {
                    if (hasFutureExercise)
                        return _context.Exercises.FirstOrDefault(x => x.ExerciseID ==
                        new Guid(associatedPreviousExercise.FutureSubStageExerciseID));
                }
                else
                {
                    if (hasPreviousExercise)
                        return _context.Exercises.FirstOrDefault(x => x.ExerciseID ==
                        new Guid(associatedPreviousExercise.PreviousSubStageExerciseID));
                }
                return associatedPreviousExercise;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ExerciseViewModel GenerateNewExerciseViewModel(Exercise exerciseConfiguration, CompletedExerciseResults completedExercise = null)
        {
            if (completedExercise == null)
                completedExercise = new CompletedExerciseResults();

            var exeriseViewModel = new ExerciseViewModel()
            {
                Id = exerciseConfiguration.ExerciseID.ToString(),
                Name = exerciseConfiguration.ExerciseName,
                Reps = exerciseConfiguration.Reps,
                Sets = exerciseConfiguration.Sets,
                Time = exerciseConfiguration.Time,
                CompletedResults = completedExercise,
                VideoLink = exerciseConfiguration.VideoLink,
                ExerciseType = (Models.ViewModels.ExerciseType)
                    Enum.Parse(typeof(Models.ViewModels.ExerciseType), exerciseConfiguration.ExerciseType.ExerciseTypeEnum.ToString(), true)
            };

            return exeriseViewModel;
        }
    }
}
