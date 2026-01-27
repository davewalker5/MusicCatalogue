using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IJobStatusManager
    {
        Task<JobStatus> AddAsync(string name, string parameters);
        Task<JobStatus> GetAsync(Expression<Func<JobStatus, bool>> predicate);
        IAsyncEnumerable<JobStatus> ListAsync(Expression<Func<JobStatus, bool>> predicate, int pageNumber, int pageSize);
        Task<JobStatus?> UpdateAsync(long id, string error);
    }
}