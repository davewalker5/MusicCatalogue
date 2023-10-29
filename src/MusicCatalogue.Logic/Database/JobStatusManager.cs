using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class JobStatusManager : IJobStatusManager
    {
        private readonly MusicCatalogueDbContext _context;

        internal JobStatusManager(MusicCatalogueDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the first job status matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<JobStatus> GetAsync(Expression<Func<JobStatus, bool>> predicate)
        {
            List<JobStatus> statuses = await ListAsync(predicate, 1, 1).ToListAsync();
#pragma warning disable CS8603
            return statuses.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all entities matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IAsyncEnumerable<JobStatus> ListAsync(Expression<Func<JobStatus, bool>> predicate, int pageNumber, int pageSize)
        {
            IAsyncEnumerable<JobStatus> results;
            if (predicate == null)
            {
                results = _context.JobStatuses
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .AsAsyncEnumerable();
            }
            else
            {
                results = _context.JobStatuses.Where(predicate).AsAsyncEnumerable();
            }

            return results;
        }

        /// <summary>
        /// Create a new job status
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<JobStatus> AddAsync(string name, string? parameters)
        {
            JobStatus status = new()
            {
                Name = name,
                Parameters = parameters,
                Start = DateTime.Now
            };

            await _context.JobStatuses.AddAsync(status);
            await _context.SaveChangesAsync();

            return status;
        }

        /// <summary>
        /// Update a job status, setting the end timestamp and error result
        /// </summary>
        /// <param name="id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public async Task<JobStatus> UpdateAsync(long id, string error)
        {
            JobStatus status = await GetAsync(x => x.Id == id);
            if (status != null)
            {
                status.End = DateTime.Now;
                status.Error = error;
                await _context.SaveChangesAsync();
            }

            return status;
        }
    }
}
