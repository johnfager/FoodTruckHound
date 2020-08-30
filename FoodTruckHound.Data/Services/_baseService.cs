using FoodTruckHound.Core.Services;
using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Data.Services
{
    public abstract class _baseService<TService> where TService : class, IService
    {
        protected ILogger<TService> _logger;

        public _baseService(ILogger<TService> logger)
        {
            logger = _logger;
        }
    }
}
