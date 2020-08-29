using FoodTruckHound.Core.Services;
using FoodTruckHound.Data.Mappings;
using FoodTruckHound.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace FoodTruckHound.Data.Services.SfGov
{
    public class SfGovMobileFoodScheduleService : IFoodTruckDataService
    {
        private static DateTime _lastRefreshedOnUtc;

        private static readonly object _lock = new object();

        public DateTime LastRefreshedOnUtc => _lastRefreshedOnUtc;


        public int RecommendedRefreshInMinutes { get; set; } = 300; // default setting for refresh rate

        public async Task<FoodTruckInfo[]> RefreshRemoteDataAsync()
        {
            // TODO: Locate and parse the CSV 

            // TODO: Get better schedule data if time (feature add)

            lock (_lock)
            {
                _lastRefreshedOnUtc = DateTime.UtcNow;
            }

            throw new NotImplementedException();
        }

        #region helpers

        // TODO: Look at populating better schedule information.

        private async Stream GetStreamAsync()
        {
            // TODO: Get teh CSV data
        }

        private IEnumerable<FoodTruckInfo> ParseCsv(Stream csvData)
        {

            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<FoodTruckInfo>(csvParserOptions, new CsvFoodTruckMapping());

            var records = csvParser.ReadFromStream(csvData, Encoding.UTF8);

            return records.Select(x => x.Result).ToList();

        }

        #endregion


    }
}
