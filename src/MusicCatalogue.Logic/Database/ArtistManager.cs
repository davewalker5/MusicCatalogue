using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Logic.Database
{
    public class ArtistManager : IArtistManager
    {
        private readonly MusicCatalogueFactory _factory;
        private readonly MusicCatalogueDbContext? _context;

        internal ArtistManager(MusicCatalogueFactory factory)
        {
            _factory = factory;
            _context = factory.Context as MusicCatalogueDbContext;
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
        /// List artists with a name beginning with the specified prefix. Preferentially use the searchable
        /// name, if available
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public async Task<List<Artist>> ListByNameAsync(string prefix)
            => await ListAsync(x => ((x.SearchableName != null) && x.SearchableName.StartsWith(prefix)) ||
                                    ((x.SearchableName == null) && x.Name.StartsWith(prefix)));

        /// <summary>
        /// Return all artists matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate)
        {
            // Load artists, albums and genres
            var artists = await _context!.Artists
                                         .Where(predicate)
                                         .OrderBy(x => x.Name)
                                         .Include(x => x.Albums)
                                         .ThenInclude(x => x.Genre)
                                         .ToListAsync();

            // ThenInclude doesn't work for this use case so load the retailers. Iterate over the artists
            foreach (var artist in artists)
            {
                // Iterate over the albums for this artist
                foreach (var album in artist.Albums)
                {
                    // Get the retailer
                    album.Retailer = await _factory.Retailers.GetAsync(x => x.Id == album.RetailerId);
                }
            }

            // Return the collection of artists
            return artists;
        }

        /// <summary>
        /// Add an artist, if they doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Artist> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var artist = await GetAsync(a => a.Name == clean);

            if (artist == null)
            {
                // Get a serchable name
                var searchableName = StringCleaner.SearchableName(clean) ?? "";
                artist = new Artist
                {
                    Name = clean,
                    SearchableName = clean != searchableName ? searchableName : null,
                };
                await _context.Artists.AddAsync(artist);
                await _context.SaveChangesAsync();
            }

            return artist;
        }
    }
}