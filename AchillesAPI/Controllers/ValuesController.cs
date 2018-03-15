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

        // GET api/values
        [HttpGet]
        public string Get()
        {
            /* Ignore this for now
            var results = new List<string>();
            var db = DBConnection.Instance();
            if(db.isConnected()){
                string query = "Select text from Test_Table";
                var cmd = new MySqlCommand(query,db.Connection);
                var reader = cmd.ExecuteReader();
                while(reader.Read()){
                    results.Add(reader.GetString(0));
                }
                db.closeConnection();
            }
            return results;*/
            return "test";
        }

        [HttpGet]
        [Route("NotTestMethod")]
        public List<Stage> TestMethod()
        {
            var items = _context.Set<Stage>();
            return items.ToList();
        }


        [HttpPost]
        public string ReturnProgressReport()
        {

            return "";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var db = DBConnection.Instance();
            if(db.isConnected()){
                int id = 0;
                string query = string.Format("Select Top 1 idTest_Table from Test_Tables order by idTest_Table DESC LIMIT 1");
                var cmd = new MySqlCommand(query,db.Connection);
                var reader = cmd.ExecuteReader();
                while(reader.Read()){
                    id = reader.GetInt32(0);
                }
                query = string.Format("Insert into Test_Tables (idTest_Table,text) values ({0},{1})",id,value);
                cmd = new MySqlCommand(query,db.Connection);
                cmd.ExecuteNonQuery();
                db.closeConnection();
            }
        }
    }
}
