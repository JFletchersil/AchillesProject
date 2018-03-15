using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillesAPI.Models.DbModels
{
    [Table("Stages")]
    public class Stage
    {
        [Key]
        public Guid StageID { get; set; }
        public int StageNumber { get; set; }
    }
}
