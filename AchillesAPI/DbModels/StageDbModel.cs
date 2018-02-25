using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.DbModels
{
    [Table("Stages")]
    public class StageDbModel
    {
        [Key]
        public string StageID { get; set; }
        public int StageNumber { get; set; }
    }
}
