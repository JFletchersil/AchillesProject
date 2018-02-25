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

namespace AchillesAPI.Controllers
{
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        // GET api/exercises
        [HttpGet]
        public string defaultPath()
        {
            return "Test";
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
    }
}