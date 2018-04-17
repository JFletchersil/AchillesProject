using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    [Table("AspNetSessionTable")]
    public class UserSession
    {
        [Key]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public DateTime ExpiresWhen { get; set; }
    }
}
