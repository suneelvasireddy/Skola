using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Skola.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skola.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SchoolsController> _logger;

        public SchoolsController(ISchoolService schoolService, IMemoryCache memoryCache, ILogger<SchoolsController> logger)
        {
            _schoolService = schoolService ?? throw new ArgumentNullException(nameof(schoolService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<List<string>>> GetSchools()
        {
            try
            {
                string cacheKey = "SchoolNames";

                // Check if the school names are in cache
                if (!_memoryCache.TryGetValue(cacheKey, out List<string> schoolNames))
                {
                    // If not in cache, fetch from service
                    _logger.LogInformation("Attempting to retrieve school names from database.");

                    schoolNames = await _schoolService.GetSchoolNamesAsync();

                    // Store in cache
                    _memoryCache.Set(cacheKey, schoolNames, TimeSpan.FromMinutes(10)); // Example: Cache for 10 minutes
                }
                else
                {
                    _logger.LogInformation("School names retrieved from cache.");
                }

                return Ok(schoolNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving school names.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
