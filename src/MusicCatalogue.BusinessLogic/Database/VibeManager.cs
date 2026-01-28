using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.BusinessLogic.Database
{
    public class VibeManager : DatabaseManagerBase, IVibeManager
    {
        internal VibeManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first vibe matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Vibe> GetAsync(Expression<Func<Vibe, bool>> predicate)
        {
            List<Vibe> vibes = await ListAsync(predicate);

#pragma warning disable CS8603
            return vibes.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all vibes matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Vibe>> ListAsync(Expression<Func<Vibe, bool>> predicate)
            => await Context.Vibes
                            .Where(predicate)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

        /// <summary>
        /// Add a vibe, if it doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Vibe> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var vibe = await GetAsync(a => a.Name == clean);

            if (vibe == null)
            {
                // Get a serchable name
                vibe = new Vibe
                {
                    Name = clean
                };

                await Context.Vibes.AddAsync(vibe);
                await Context.SaveChangesAsync();
            }

            return vibe;
        }

        /// <summary>
        /// Update the properties of the specified vibe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Vibe?> UpdateAsync(int id, string name)
        {
            var vibe = Context.Vibes.FirstOrDefault(x => x.Id == id);
            if (vibe != null)
            {
                // Save the changes
                vibe.Name = StringCleaner.Clean(name)!;
                await Context.SaveChangesAsync();
            }

            return vibe;
        }

        /// <summary>
        /// Delete the vibe with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the vibe record and check it exists
            var vibe = await GetAsync(x => x.Id == id);
            if (vibe != null)
            {
                // If there's any artists associated with the vibe, they can't be deleted
                var artists = Context.Artists.Where(x => x.VibeId == id);
                if (artists.Any())
                {
                    var message = $"Cannot delete vibe '{vibe.Name} with Id {id} with artists associated with it";
                    throw new VibeInUseException(message);
                }

                // Delete the vibe record and save changes
                Factory.Context.Remove(vibe);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}