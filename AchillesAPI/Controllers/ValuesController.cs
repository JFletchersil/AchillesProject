using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Utility;
using MySql.Data.MySqlClient;
using AchillesAPI.Contexts;
using AchillesAPI.Models.DbModels;
using Microsoft.AspNetCore.Cors;

namespace AchillesAPI.Controllers
{
    [EnableCors("MyPolicy")]
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
