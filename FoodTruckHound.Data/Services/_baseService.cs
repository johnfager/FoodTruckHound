using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Data.Services
{
    public class _baseService
    {
        protected ILogger _logger;

        public _baseService(ILogger logger)
        {
            logger = _logger;
        }
    }
}
