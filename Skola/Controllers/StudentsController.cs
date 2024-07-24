using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skola.API.Services;
using Skola.API.Models;

namespace Skola.API.Controllers
{
    /// <summary>
    /// Controller for handling student-related API endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentsController"/> class.
        /// </summary>
        /// <param name="studentService">The student service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a paged list of students by school name.
        /// </summary>
        /// <param name="schoolName">The name of the school to filter students.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of students per page.</param>
        /// <returns>A paged result of students filtered by school name.</returns>
        /// <response code="200">Returns the paged list of students.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("students")]
        public async Task<ActionResult<PagedResult<StudentResult>>> GetStudentsBySchoolName(
            string schoolName,
            int pageNumber = 1,
            int pageSize = 10) // Default page size, adjust as needed.
        {
            _logger.LogInformation("GetStudentsBySchoolName called with parameters schoolName: {SchoolName}, pageNumber: {PageNumber}, pageSize: {PageSize}", schoolName, pageNumber, pageSize);

            try
            {
                var result = await _studentService.GetStudentsBySchoolNameAsync(schoolName, pageNumber, pageSize);
                //return Ok(result);

                //resturn json mssg
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetStudentsBySchoolName with parameters schoolName: {SchoolName}, pageNumber: {PageNumber}, pageSize: {PageSize}", schoolName, pageNumber, pageSize);
                //return StatusCode(500, $"Internal server error: {ex.Message}");

                //return json mssg 
                return StatusCode(500, new { success = false, message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}