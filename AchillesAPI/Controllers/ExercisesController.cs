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
using Microsoft.Extensions.Options;
using AchillesAPI.Models.AppOptions;
using AchillesAPI.Models.Helper;

namespace AchillesAPI.Controllers
{
    /// <summary>
    /// The controller that manages all exercise related functionality within the application
    /// </summary>
    /// <seealso cref="Controller" />
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AngularDbContext _context;
        /// <summary>
        /// The authentication context
        /// </summary>
        private readonly ApplicationDbContext _authContext;
        /// <summary>
        /// The authentication helper
        /// </summary>
        private readonly AuthenticationHelper authenticationHelper;
        /// <summary>
        /// The session expired view model
        /// This easily and quickly allows the application to respond that the user must log in
        /// </summary>
        private readonly SessionExpiredViewModel sessionExpired;
        /// <summary>
        /// The application options
        /// </summary>
        private readonly ApplicationOptions _appOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExercisesController"/> class.
        /// </summary>
        /// <param name="context">The context to access the data portion of the database.</param>
        /// <param name="appContext">The authentication context.</param>
        /// <param name="getAppOptions">Gets app application options using a standard model</param>
        public ExercisesController(AngularDbContext appContext, ApplicationDbContext authContext,
            IOptions<ApplicationOptions> getAppOptions)
        {
            _context = appContext;
            _authContext = authContext;
            _appOptions = getAppOptions.Value;
            authenticationHelper = new AuthenticationHelper();
            sessionExpired = new SessionExpiredViewModel()
            {
                ConsoleMessage = "User Session has Expired, Please Log into the Application",
                IsError = true,
                UserInformationMessage = "You are required to log back into the Application"
            };
        }

        // GET api/exercises/additional
        /// <summary>
        /// Gets the additional exercises.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <returns>A list of additional exercises.</returns>
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

        /// <summary>
        /// Gets the daily exercises.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <returns>An <see cref="IActionResult"/> that contains a List of <see cref="ExerciseViewModel"/></returns>
        [HttpGet]
        [Route("GetDailyExercises")]
        public IActionResult GetDailyExercises(Guid sessionID)
        {
            if (!authenticationHelper.VerifySession(sessionID, _authContext))
                return BadRequest(sessionExpired);

            var userID = authenticationHelper.DerriveUserIdFromSessionId(sessionID, _authContext);
            // If we have previous exercises for today, then return those exercises and give the results back to the user.
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
                return Ok(exerciseViewModels.OrderByDescending(x => x.Name));
            }
            else
            {
                var viewModels = GetNewExercisesFromDatabaseDeterminingAppropriateSubstaging(userID).Select(x => GenerateNewExerciseViewModel(x));

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
                return Ok(viewModels.OrderByDescending(x => x.Name));
            }
        }

        /// <summary>
        /// Saves all daily exercises.
        /// </summary>
        /// <param name="exerciseViewModel">The exercise view model.</param>
        /// <returns><see cref="IActionResult"/> that will either be <see cref="OkResult"></see> <see cref="BadRequestResult"></see></returns>
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

        /// <summary>
        /// Saves the single daily exercise.
        /// </summary>
        /// <param name="exerciseViewModel">The exercise view model.</param>
        /// <returns><see cref="IActionResult"/> that will either be <see cref="OkResult"> <see cref="BadRequestResult"></see></returns>
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

        /// <summary>
        /// Gets the exercises by user stage.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="allExercises">if set to <c>true</c> [all exercises].</param>
        /// <returns>A list of <see cref="Exercise"/></returns>
        private List<Exercise> GetExercisesByUserStage(Guid userID, bool allExercises = true)
        {
            // Gets the current user level
            var userLevel = _authContext.Users.FirstOrDefault(x => x.Id == userID.ToString()).UserLevel;
            // Gets the stage ID for the users current level
            var exerciseLevelId = _context.Stages.FirstOrDefault(x => x.StageNumber == userLevel).StageID;
            // Gets all of the exercises, from the exercise stage table, that have the current stage ID
            var exercisesOfLevel = _context.ExerciseStages.Where(x => x.StageID == exerciseLevelId);

            // Depending on need, either include all exercises in order to filter based off previous exercises
            // performed, or only return exercises that are at the start of a chain of exercises
            if (allExercises)
            {
                // Get all of the exercises within the database at the users level, 
                // including the exercise type in the object
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


        /// <summary>
        /// Gets all exercises within the correct stage from last exercise session of a given user.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>A <see cref="UserExerciseAndExercisePairLists"/> Containing a list of <see cref="UserExercise"/> and a list of <see cref="Exercise"/></returns>
        private UserExerciseAndExercisePairLists GetAllExercisesWithinCorrectStageFromLastExerciseSession(Guid userID)
        {
            var exercisesWithinStage = GetExercisesByUserStage(userID);

            // Retrieve the most recent date that the exercises were performed, take this date as the date that the 
            // Application should use to find user exercises when attempting to get the progress made by the user
            var previousDate = _context.UserExercises.Where(x => x.AuthUserID == userID && x.DateTime.Date != DateTime.Now.Date)
                .OrderByDescending(x => x.DateTime).FirstOrDefault().DateTime;
            // Based off the previous, get all the exercise results from the user results table that match the ID of the user, and match the datetime of the 
            // last time the user performed exercises
            var previousUserExercises = _context.UserExercises.Where(x => (x.DateTime.Date == previousDate.Date) && (x.AuthUserID == userID)).ToList();
            // Based off the previous, return all exercises that match the exercises within the user exercise table. This ensures
            // that only the exercises that are relevant to the user are present.
            var previousExercisesFromExerciseTable = exercisesWithinStage.Where(y => previousUserExercises.Any(x => x.ExerciseStageID == y.ExerciseID)).ToList();
            return new UserExerciseAndExercisePairLists()
            {
                PreviousStandardExercises = previousExercisesFromExerciseTable,
                PreviousUserExercises = previousUserExercises
            };
        }

        /// <summary>
        /// Evaluates if user moves ahead or behind on an exercise or remains where they are.
        /// </summary>
        /// <param name="associatedPreviousExercise">The associated previous exercise.</param>
        /// <param name="exerciseProgress">The exercise progress.</param>
        /// <param name="completedItems">The completed exercise items.</param>
        /// <param name="completedLimit">The goal that the completed exercise items are aiming for.</param>
        /// <returns>A <see cref="Exercise"/> that is either ahead of the current exercise, behind, or the same exercise as given</returns>
        /// <exception cref="ArgumentNullException">CompletedLimit or CompletedItems are null</exception>
        private Exercise EvaluateIfUserMovesAheadOrBehindExercises(Exercise associatedPreviousExercise,
            UserExercise exerciseProgress, List<double?> completedItems, double? completedLimit)
        {

            var hasPreviousExercise = !string.IsNullOrWhiteSpace(associatedPreviousExercise.PreviousSubStageExerciseID);
            var hasFutureExercise = !string.IsNullOrWhiteSpace(associatedPreviousExercise.FutureSubStageExerciseID);

            // Basically, check to see if the user needs to be moved ahead or back in the exercises
            // First check, make sure that we actually have some results in the completedItems or completedLimit
            if (completedLimit == null || completedItems == null || !completedLimit.HasValue || !completedItems.All(x => x.HasValue))
            {
                throw new ArgumentNullException("CompletedLimit or CompletedItems are null");
            }

            try
            {
                // Second Job, check if the user has completed a given exercise or not.
                // The function will check all items match the predicate given
                // This predicate states that the number of sets must match the sets within the exercise,
                // and that the value must match the completedLimit
                var hasCompleted = completedItems.All(x => x.Value == completedLimit)
                    && completedItems.Count() == associatedPreviousExercise.Sets;

                // Third job, check to see if the above value was true or not
                if (hasCompleted)
                {
                    // It was true, check that the previous exercise has the ability to move ahead,
                    // if it is possible for the user to move ahead, then we need to return that exercise, instead of the current exercise
                    if (hasFutureExercise)
                        return _context.Exercises.FirstOrDefault(x => x.ExerciseID ==
                        new Guid(associatedPreviousExercise.FutureSubStageExerciseID));
                }
                else
                {
                    // It was true, check that the previous exercise has the ability to move back,
                    // if it is possible for the user to move back, then we need to return that exercise, instead of the current exercise
                    if (hasPreviousExercise)
                        return _context.Exercises.FirstOrDefault(x => x.ExerciseID ==
                        new Guid(associatedPreviousExercise.PreviousSubStageExerciseID));
                }
                // Else, just return the given exercise
                return associatedPreviousExercise;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generates the new exercise view model.
        /// </summary>
        /// <param name="exerciseConfiguration">The exercise configuration.</param>
        /// <param name="completedExercise">The completed exercise.</param>
        /// <returns>A new <see cref="ExerciseViewModel"/></returns>
        private ExerciseViewModel GenerateNewExerciseViewModel(Exercise exerciseConfiguration, CompletedExerciseResults completedExercise = null)
        {
            if (completedExercise == null)
            {
                completedExercise = new CompletedExerciseResults();
                completedExercise.ExerciseId = exerciseConfiguration.ExerciseID;
            }

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

        /// <summary>
        /// Gets the daily new exercises from the database, determining appropriate substaging of the exercises, within the users current stage.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>A list of <see cref="Exercise"/> that the user should exercise with on the current day</returns>
        private List<Exercise> GetNewExercisesFromDatabaseDeterminingAppropriateSubstaging(Guid userID)
        {
            var exercises = new List<Exercise>();
            // Check if this is a new user or not, this checks to see if the user has ever used the application prior
            // This must be performed, as sub-staging within an individual stage requires previous exercises to 
            // have been done
            var isOldUser = _context.UserExercises.Any(x => x.AuthUserID == userID);
            if (isOldUser)
            {
                // Get all the exercises from within the current stage, based on the last exercise session
                var exercisesFromLastExerciseSession = GetAllExercisesWithinCorrectStageFromLastExerciseSession(userID);

                // For each user exercise, derive if the user can move ahead, or must go back.
                foreach (var userExercise in exercisesFromLastExerciseSession.PreviousUserExercises)
                {
                    // Get the exercise associated with the user exercise being evaluated
                    var associatedPreviousExercise = exercisesFromLastExerciseSession.PreviousStandardExercises
                        .FirstOrDefault(x => x.ExerciseID == userExercise.ExerciseStageID);

                    // Get the results from the ResultsJSON and convert it into the appropriate model
                    var exerciseProgress = JsonConvert.DeserializeObject<CompletedExerciseResults>(userExercise.ResultsJSON);
                    switch (associatedPreviousExercise.ExerciseType.ExerciseTypeEnum)
                    {
                        case ExerciseTypes.Timed:
                            exercises.Add(EvaluateIfUserMovesAheadOrBehindExercises(associatedPreviousExercise,
                                userExercise, exerciseProgress.CompletedTimes, associatedPreviousExercise.Time));
                            break;
                        case ExerciseTypes.RepsSets:
                            exercises.Add(EvaluateIfUserMovesAheadOrBehindExercises(associatedPreviousExercise,
                                userExercise, exerciseProgress.CompletedReps, associatedPreviousExercise.Reps));
                            break;
                        default:
                            break;
                    }
                }

                // If the number of daily exercises does not match the current number of exercises, then fill
                // in the rest and ensure that there are always at least the same number of exercises each day.
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
                // Else, just take the number of daily exercises from the current stage and make those the exercises
                exercises = GetExercisesByUserStage(userID, false).Take(_appOptions.DailyExercises).ToList();
            }
            return exercises;
        }
    }
}
