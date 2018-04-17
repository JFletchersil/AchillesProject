using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Utility;
using MySql.Data.MySqlClient;
using AchillesAPI.Contexts;
using AchillesAPI.Models.DbModels;

namespace AchillesAPI.Controllers
{
    [Route("api/[controller]")] 
    public class ValuesController : Controller
    {

        private readonly AngularDbContext _context;

        public ValuesController(AngularDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string ReturnProgressReport()
        {
            return "";
        }
    }
}
