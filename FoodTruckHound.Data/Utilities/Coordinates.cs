using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTruckHound.Data.Utilities
{

    public class Coordinates
    {
        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
