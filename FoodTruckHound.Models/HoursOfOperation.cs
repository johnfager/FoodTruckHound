using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTruckHound.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HoursOfOperation
    {
        public string DayOfWeek { get; set; }

        public int Start24 { get; set; }

        public int End24 { get; set; }
    }
}
