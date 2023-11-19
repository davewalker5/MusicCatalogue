using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    internal class GenreManager : DatabaseManagerBase, IGenreManager
    {
        internal GenreManager(IMusicCatalogueFactory factory) : base(factory)
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
            => await Context.Genres
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
                await Context.Genres.AddAsync(genre);
                await Context.SaveChangesAsync();
            }

            return genre;
        }

        /// <summary>
        /// Update a genre given its ID
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Genre?> UpdateAsync(int genreId, string name)
        {
            var genre = Context.Genres.FirstOrDefault(x => x.Id == genreId);
            if (genre != null)
            {
                genre.Name = StringCleaner.Clean(name)!;
                await Context.SaveChangesAsync();
            }
            return genre;
        }

        /// <summary>
        /// Delete a genre given its ID
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int genreId)
        {
            // Check the retailer exists
            var enre = await GetAsync(a => a.Id == genreId);
            if (enre != null)
            {
                // Check the retailer isn't in use
                var albums = await Factory.Albums.ListAsync(x => x.GenreId == genreId);
                if (albums.Any())
                {
                    var message = $"Genre '{enre.Name} with Id {genreId} is in use and cannot be deleted";
                    throw new GenreInUseException(message);
                }

                // Delete the retailer
                Context.Genres.Remove(enre);
                await Context.SaveChangesAsync();
            }

        }
    }
}