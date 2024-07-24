using Skola.API.Models;
using System.Threading.Tasks;

namespace Skola.API.Services
{
    /// <summary>
    /// Interface for student service operations.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Retrieves a paged list of students by school name asynchronously.
        /// </summary>
        /// <param name="schoolName">The name of the school to filter students.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of students per page.</param>
        /// <returns>A task representing the asynchronous operation, returning a paged result of students.</returns>
        Task<PagedResult<StudentResult>> GetStudentsBySchoolNameAsync(string schoolName, int pageNumber, int pageSize);
    }
}
