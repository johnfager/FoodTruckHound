using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Data.Repositories
{
    public class _baseRepository
    {
        protected ILogger _logger;

        public _baseRepository(ILogger logger)
        {
            logger = _logger;
        }
    }
}
