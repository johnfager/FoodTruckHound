using FoodTruckHound.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Data.Repositories
{
    public abstract class _baseRepository<TRepository> where TRepository : class, IRepository
    {
        protected ILogger<TRepository> _logger;

        public _baseRepository(ILogger<TRepository> logger)
        {
            _logger = logger;
        }

    }
}
