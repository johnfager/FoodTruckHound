using FoodTruckHound.Core.Configuration;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Data.Mappings;
using FoodTruckHound.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace FoodTruckHound.Data.Services.SfGov
{
    public class SfGovMobileFoodScheduleService : _baseService<SfGovMobileFoodScheduleService>, IFoodTruckDataService
    {

        private static readonly object _lock = new object();

        // use a static client to properly pool connections
        private static HttpClient _httpClient;


        private SfGovEndpoints _sfGovEndpoints;

        public SfGovMobileFoodScheduleService(IOptions<SfGovEndpoints> options, ILogger<SfGovMobileFoodScheduleService> logger) : base(logger)
        {
            _sfGovEndpoints = options.Value;

            // instantiate the clien if not yet done
            lock (_lock)
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient()
                    {
                        BaseAddress = new Uri(_sfGovEndpoints.RootUrl)
                    };
                }
            }

        }

        public async Task<List<FoodTruckInfo>> RefreshRemoteDataAsync()
        {

            _logger.LogTrace($"{nameof(SfGovMobileFoodScheduleService)}.{nameof(RefreshRemoteDataAsync)} - Starting process");

            var stream = await _httpClient.GetStreamAsync(_sfGovEndpoints.FoodTruckCsvUrl);

            _logger.LogTrace($"{nameof(SfGovMobileFoodScheduleService)}.{nameof(RefreshRemoteDataAsync)} - Stream aquired to '{_sfGovEndpoints.FoodTruckCsvUrl}'");

            var data = this.ParseCsv(stream);

            // TODO: Get better schedule data if time (feature add)

            return data;
        }

        #region helpers

        // TODO: Look at populating better schedule information.

        private List<FoodTruckInfo> ParseCsv(Stream csvData)
        {
            _logger.LogTrace($"{nameof(SfGovMobileFoodScheduleService)}.{nameof(ParseCsv)} - Starting CsvParser");

            // setup the parsing options
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<FoodTruckInfo>(csvParserOptions, new CsvFoodTruckMapping()); // mapping created based off of CSV documentation

            var records = csvParser.ReadFromStream(csvData, Encoding.UTF8);
            var data = records.Select(x => x.Result).ToList();

            _logger.LogTrace($"{nameof(SfGovMobileFoodScheduleService)}.{nameof(ParseCsv)} - Parsed '{data.Count}' records");

            return data;
        }

        #endregion


    }
}
