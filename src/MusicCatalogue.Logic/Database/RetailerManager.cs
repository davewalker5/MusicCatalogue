using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    internal class RetailerManager : DatabaseManagerBase, IRetailerManager
    {
        internal RetailerManager(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Return the first retailer matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Retailer> GetAsync(Expression<Func<Retailer, bool>> predicate)
        {
            List<Retailer> retailers = await ListAsync(predicate);

#pragma warning disable CS8603
            return retailers.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all retailers matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Retailer>> ListAsync(Expression<Func<Retailer, bool>> predicate)
            => await _context.Retailers
                             .Where(predicate)
                             .OrderBy(x => x.Name)
                             .ToListAsync();

        /// <summary>
        /// Add a retailer, if they doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Retailer> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var retailer = await GetAsync(a => a.Name == clean);

            if (retailer == null)
            {
                retailer = new Retailer
                {
                    Name = clean
                };
                await _context.Retailers.AddAsync(retailer);
                await _context.SaveChangesAsync();
            }

            return retailer;
        }
    }
}
