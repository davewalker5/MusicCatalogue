using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    internal class RetailerManager : IRetailerManager
    {
        private readonly MusicCatalogueFactory _factory;
        private readonly MusicCatalogueDbContext? _context;

        internal RetailerManager(MusicCatalogueFactory factory)
        {
            _factory = factory;
            _context = factory.Context as MusicCatalogueDbContext;
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
            => await _context!.Retailers
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
                await _context!.Retailers.AddAsync(retailer);
                await _context!.SaveChangesAsync();
            }

            return retailer;
        }

        /// <summary>
        /// Update a retailer given their ID
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Retailer?> UpdateAsync(int retailerId, string name)
        {
            var retailer = await GetAsync(a => a.Id == retailerId);
            if (retailer != null)
            {
                retailer.Name = StringCleaner.Clean(name)!;
                await _context!.SaveChangesAsync();
            }
            return retailer;
        }

        /// <summary>
        /// Delete a retailer given their ID
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int retailerId)
        {
            // Check the retailer exists
            var retailer = await GetAsync(a => a.Id == retailerId);
            if (retailer != null)
            {
                // Check the retailer isn't in use
                var albums = await _factory.Albums.ListAsync(x => x.RetailerId == retailerId);
                if (albums.Any())
                {
                    var message = $"Retailer '{retailer.Name} with Id {retailerId} is in use and cannot be deleted";
                    throw new RetailerInUseException(message);
                }

                // Delete the retailer
                _context!.Retailers.Remove(retailer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
