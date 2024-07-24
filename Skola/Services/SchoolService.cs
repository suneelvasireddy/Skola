using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Skola.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skola.API.Services
{
    /// <summary>
    /// Service class for handling operations related to schools.
    /// </summary>
    public class SchoolService : ISchoolService
    {
        private readonly Skola24Context _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SchoolService> _logger;

        // Cache key
        private const string SchoolNamesCacheKey = "SchoolNames";

        /// <summary>
        /// Constructor for initializing SchoolService with a database context, memory cache, and logger.
        /// </summary>
        /// <param name="context">Database context instance (Skola24Context).</param>
        /// <param name="cache">Memory cache instance.</param>
        /// <param name="logger">Logger instance.</param>
        public SchoolService(Skola24Context context, IMemoryCache cache, ILogger<SchoolService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<List<string>> GetSchoolNamesAsync()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve school names.");

                // Try to get the school names from the cache
                if (!_cache.TryGetValue(SchoolNamesCacheKey, out List<string> schoolNames))
                {
                    _logger.LogInformation("School names not found in cache, retrieving from database.");

                    // If not found in cache, retrieve from database
                    schoolNames = await _context.Schools
                        .Select(s => s.Name)
                        .ToListAsync();

                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    // Save data in cache
                    _cache.Set(SchoolNamesCacheKey, schoolNames, cacheEntryOptions);

                    _logger.LogInformation("School names retrieved from database and cached. Total Schools {0}",schoolNames.Count);
                }
                else
                {
                    _logger.LogInformation("School names retrieved from cache.");
                }

                return schoolNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving school names.");
                throw; // Re-throw exception to propagate it to the caller
            }
        }
    }
}
