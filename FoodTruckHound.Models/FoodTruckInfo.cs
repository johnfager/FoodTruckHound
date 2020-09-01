namespace FoodTruckHound.Models
{
    public class FoodTruckInfo
    {  

        public int LocationId { get; set; }

        public string DisplayName { get; set; }

        public string FacilityType { get; set; }

        public string LocationDescription { get; set; }

        public string Address { get; set; }

        public string Permit { get; set; }

        public string Status { get; set; }

        public string FoodItems { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        /// <summary>
        /// The URL of the food truck's schedule.
        /// </summary>
        /// <remarks>This URL is populated but is very slow responding and troubling. Looking for better schedule information.</remarks>
        public string ScheduleUrl { get; set; }
    }
}
