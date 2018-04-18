using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels.ErrorModels
{
    public class ErrorMessageViewModel
    {
        public bool IsError { get; set; } = true;
        public string ConsoleMessage { get; set; }
        public string UserInformationMessage { get; set; }
    }

    public class SessionExpiredViewModel : ErrorMessageViewModel
    {
        public bool HasExpired { get { return true; } }
    }
}
