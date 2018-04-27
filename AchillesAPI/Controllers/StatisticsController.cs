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
    /// The controller responsible controlling all the actions associated with the statistics of individual users.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class StatisticsController : Controller
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
        /// The session expired
        /// </summary>
        private readonly SessionExpiredViewModel sessionExpired;
        /// <summary>
        /// The application options
        /// </summary>
        private readonly ApplicationOptions _appOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="optionsAccessor">The options accessor.</param>
        public StatisticsController(AngularDbContext context, ApplicationDbContext appContext,
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

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <returns>An <see cref="IActionResult"/> containing all of the relevant statisitical information.</returns>
        [HttpGet]
        [Route("GetStatistics")]
        public IActionResult GetStatistics(Guid sessionID)
        {
            //Verify the session
            if (!authenticationHelper.VerifySession(sessionID, _authContext)) return BadRequest(sessionExpired);

            //Get the user ID
            var userID = authenticationHelper.DerriveUserIdFromSessionId(sessionID, _authContext);

            //Get the stats from the database
            var statsInfo = (from ue in _context.UserExercises
                             join ex in _context.Exercises on ue.ExerciseStageID equals ex.ExerciseID
                             where ue.AuthUserID == userID
                             select new StatisticsViewModel
                             {
                                 Date = ue.DateTime,
                                 Results = ue.ResultsJSON,
                                 Exercise = ex.ExerciseName,
                                 Sets = ex.Sets,
                                 Reps = ex.Reps,
                                 Time = ex.Time
                             }).ToList();

            return Ok(statsInfo);
        }
    }
}