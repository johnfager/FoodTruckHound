using FoodTruckHound.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace FoodTruckHound.Data.Mappings
{
    internal class CsvFoodTruckMapping : CsvMapping<FoodTruckInfo>
    {

        public CsvFoodTruckMapping() : base()
        {
            // TODO: Clean up

            //MapProperty(0, x => x.Make);
            //MapProperty(1, x => x.Model);
            //MapProperty(2, x => x.Type, new EnumConverter<AutomobileType>());
            //MapProperty(3, x => x.Year);
            //MapProperty(4, x => x.Price);
            //MapProperty(5, x => x.Comment, new AutomobileCommentTypeConverter());
        }
    }

    //internal class AutomobileCommentTypeConverter : ITypeConverter<AutomobileComment>
    //{
    //    public Type TargetType => typeof(AutomobileComment);

    //    public bool TryConvert(string value, out AutomobileComment result)
    //    {
    //        result = new AutomobileComment
    //        {
    //            Comment = value
    //        };
    //        return true;
    //    }
    //}
}
