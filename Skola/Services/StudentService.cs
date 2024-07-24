using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Skola.API.Data;
using Skola.API.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Skola.API.Services
{
    /// <summary>
    /// Implementation of IStudentService for student-related operations.
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly Skola24Context _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(Skola24Context context, ILogger<StudentService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a paged list of students by school name asynchronously.
        /// </summary>
        /// <param name="schoolName">The name of the school to filter students.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of students per page.</param>
        /// <returns>A task representing the asynchronous operation, returning a paged result of students.</returns>
        public async Task<PagedResult<StudentResult>> GetStudentsBySchoolNameAsync(string schoolName, int pageNumber, int pageSize)
        {
            try
            {
                var totalCountParam = new SqlParameter("@TotalStudents", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                var students = await _context.StudentResults
                    .FromSqlRaw("EXECUTE dbo.GetAbsentStudentsBySchoolName @SchoolName, @PageNumber, @PageSize, @TotalStudents OUTPUT",
                        new SqlParameter("@SchoolName", schoolName),
                        new SqlParameter("@PageNumber", pageNumber),
                        new SqlParameter("@PageSize", pageSize),
                        totalCountParam)
                    .ToListAsync();

                var totalCount = (int)totalCountParam.Value;
                _logger.LogInformation("Students retreived Total Students {0}", totalCount);

                return new PagedResult<StudentResult>(students, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching students by school name: {SchoolName}, pageNumber: {PageNumber}, pageSize: {PageSize}", schoolName, pageNumber, pageSize);
                throw; // Propagate the exception to the caller
            }
        }
    }
}
