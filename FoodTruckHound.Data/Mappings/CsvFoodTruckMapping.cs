using FoodTruckHound.Models;
using TinyCsvParser.Mapping;

namespace FoodTruckHound.Data.Mappings
{
    internal class CsvFoodTruckMapping : CsvMapping<FoodTruckInfo>
    {

        public CsvFoodTruckMapping() : base()
        {
            MapProperty(0, x => x.LocationId);
            MapProperty(1, x => x.DisplayName);
            MapProperty(2, x => x.FacilityType);
            MapProperty(4, x => x.LocationDescription);
            MapProperty(5, x => x.Address);
            MapProperty(9, x => x.Permit);
            MapProperty(10, x => x.Status);
            MapProperty(11, x =>  x.FoodItems);
            MapProperty(14, x => x.Latitude);
            MapProperty(15, x => x.Longitude);
        }
    }
}
