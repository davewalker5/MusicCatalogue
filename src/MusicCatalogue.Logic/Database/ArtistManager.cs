using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class ArtistManager : DatabaseManagerBase, IArtistManager
    {
        internal ArtistManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first artist matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="loadAlbums"></param>
        /// <returns></returns>
        public async Task<Artist> GetAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums)
        {
            List<Artist> artists = await ListAsync(predicate, loadAlbums);

#pragma warning disable CS8603
            return artists.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all artists matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="loadAlbums"></param>
        /// <returns></returns>
        public async Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums)
        {
            // Load artists, albums and genres
            var artists = await Context.Artists
                                       .Where(predicate)
                                       .OrderBy(x => x.Name)
                                       .ToListAsync();

            // Load the albums for each artist
            foreach (var artist in artists)
            {
                artist.Albums = await Factory.Albums.ListAsync(x => x.ArtistId == artist.Id);
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
            var artist = await GetAsync(a => a.Name == clean, false);

            if (artist == null)
            {
                // Get a serchable name
                var searchableName = StringCleaner.SearchableName(clean) ?? "";
                artist = new Artist
                {
                    Name = clean,
                    SearchableName = clean != searchableName ? searchableName : null,
                };
                await Context.Artists.AddAsync(artist);
                await Context.SaveChangesAsync();
            }

            return artist;
        }

        /// <summary>
        /// Update the properties of the specified artist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Artist?> UpdateAsync(int id, string name)
        {
            var artist = Context.Artists.FirstOrDefault(x => x.Id == id);
            if (artist != null)
            {
                // Save the changes
                artist.Name = StringCleaner.Clean(name)!;
                await Context.SaveChangesAsync();

                // Reload the artist to retrieve related entities
                artist = await GetAsync(x => x.Id == id, true);
            }

            return artist;
        }

        /// <summary>
        /// Delete the artist with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the artist record and check it exists
            var artist = await GetAsync(x => x.Id == id, true);
            if (artist != null)
            {
                // If they have any albums, they can't be deleted
                if (artist.Albums!.Any())
                {
                    var message = $"Artist '{artist.Name} with Id {id} has albums and cannot be deleted";
                    throw new ArtistInUseException(message);
                }

                // Delete the artist record and save changes
                Factory.Context.Remove(artist);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}