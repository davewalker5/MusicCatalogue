using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    internal class GenreManager : DatabaseManagerBase, IGenreManager
    {
        internal GenreManager(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Return the first genre matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Genre> GetAsync(Expression<Func<Genre, bool>> predicate)
        {
            List<Genre> genres = await ListAsync(predicate);

#pragma warning disable CS8603
            return genres.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all genres matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Genre>> ListAsync(Expression<Func<Genre, bool>> predicate)
            => await _context.Genres
                             .Where(predicate)
                             .OrderBy(x => x.Name)
                             .ToListAsync();

        /// <summary>
        /// Add a genre, if it doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Genre> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var genre = await GetAsync(a => a.Name == clean);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = clean
                };
                await _context.Genres.AddAsync(genre);
                await _context.SaveChangesAsync();
            }

            return genre;
        }
    }
}
