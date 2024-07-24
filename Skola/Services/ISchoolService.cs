using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skola.API.Services
{
    /// <summary>
    /// Interface for school service operations.
    /// </summary>
    public interface ISchoolService
    {
        /// <summary>
        /// Retrieves a list of school names asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning a list of school names.</returns>
        Task<List<string>> GetSchoolNamesAsync();
    }
}
