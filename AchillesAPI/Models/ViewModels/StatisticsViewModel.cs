using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public DateTime Date { get; set; }
        public string Results { get; set; }
        public string Exercise {get;set;}
        public double? Reps { get; set; }
        public double? Sets { get; set; }
        public double? Time { get; set; }
    }
}
