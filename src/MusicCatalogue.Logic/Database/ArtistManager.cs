using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class ArtistManager : DatabaseManagerBase, IArtistManager
    {
        internal ArtistManager(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Return the first artist matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Artist> GetAsync(Expression<Func<Artist, bool>> predicate)
        {
            List<Artist> artists = await ListAsync(predicate);

#pragma warning disable CS8603
            return artists.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all artists matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate)
            => await _context.Artists.Where(predicate).ToListAsync();

        /// <summary>
        /// Add an artist, if they doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Artist> AddAsync(string name)
        {
            var artist = await GetAsync(a => a.Name == name);

            if (artist == null)
            {
                artist = new Artist
                {
                    Name = _textInfo.ToTitleCase(name)
                };
                await _context.Artists.AddAsync(artist);
                await _context.SaveChangesAsync();
            }

            return artist;
        }
    }
}