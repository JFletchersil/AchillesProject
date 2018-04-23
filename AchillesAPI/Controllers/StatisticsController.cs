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
    public class StatisticsController : Controller
    {
        private readonly AngularDbContext _context;
        private readonly ApplicationDbContext _authContext;
        private readonly AuthenticationHelper authenticationHelper;
        private readonly SessionExpiredViewModel sessionExpired;
        private readonly ApplicationOptions _appOptions;

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