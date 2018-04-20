using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    public class ValidEditUserViewModel
    {
        public Guid SessionId { get; set; }
        public UserViewModel UserModel { get; set;}
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int UserLevel { get; set; }
    }
}
