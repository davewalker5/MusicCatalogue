using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.BusinessLogic.Database
{
    public class AlbumManager : DatabaseManagerBase, IAlbumManager
    {
        internal AlbumManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first album matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Album> GetAsync(Expression<Func<Album, bool>> predicate)
        {
            List<Album> albums = await ListAsync(predicate);

#pragma warning disable CS8603
            return albums.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all albums matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Album>> ListAsync(Expression<Func<Album, bool>> predicate)
            => await Context.Albums
                            .Where(predicate)
                            .OrderBy(x => x.Title)
                            .Include(x => x.Genre)
                            .Include(x => x.Retailer)
                            .Include(x => x.Tracks)
                            .ToListAsync();

        /// <summary>
        /// Return a randomly selected album from those matching the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<Album?> GetRandomAsync(Expression<Func<Album, bool>> predicate)
        {
            Album? album = null;

            // Get a list of albums matching the predicate
            var albums = await ListAsync(predicate);

            // If there are any, pick one at random
            var numberOfAlbums = albums.Count;
            if (numberOfAlbums > 0)
            {
                var index = new Random().Next(0, numberOfAlbums);
                album = albums[index];
            }

            return album;
        }

        /// <summary>
        /// Add an album, if it doesn't already exist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="genreId"></param>
        /// <param name="title"></param>
        /// <param name="released"></param>
        /// <param name="coverUrl"></param>
        /// <param name="isWishlistItem"></param>
        /// <param name="purchased"></param>
        /// <param name="price"></param>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task<Album> AddAsync(
            int artistId,
            int genreId,
            string title,
            int? released,
            string? coverUrl,
            bool? isWishlistItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId)
        {
            var clean = StringCleaner.Clean(title)!;
            var album = await GetAsync(a => (a.ArtistId == artistId) && (a.Title == clean));

            if (album == null)
            {
                // Add the album
                album = new Album
                {
                    ArtistId = artistId,
                    GenreId = genreId,
                    Title = clean,
                    Released = released,
                    CoverUrl = StringCleaner.RemoveInvalidCharacters(coverUrl),
                    IsWishListItem = isWishlistItem,
                    Purchased = purchased,
                    Price = price,
                    RetailerId = retailerId
                };
                await Context.Albums.AddAsync(album);
                await Context.SaveChangesAsync();

                // Now re-retrieve it to populate related entities
                album = await GetAsync(x => x.Id == album.Id);
            }

            return album;
        }

        /// <summary>
        /// Update the properties of the specified album
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="artistId"></param>
        /// <param name="genreId"></param>
        /// <param name="title"></param>
        /// <param name="released"></param>
        /// <param name="coverUrl"></param>
        /// <param name="isWishlistItem"></param>
        /// <param name="purchased"></param>
        /// <param name="price"></param>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task<Album?> UpdateAsync(
            int albumId,
            int artistId,
            int genreId,
            string title,
            int? released,
            string? coverUrl,
            bool? isWishlistItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId)
        {
            var album = Context.Albums.FirstOrDefault(x => x.Id == albumId);
            if (album != null)
            {
                // Apply the changes
                album.ArtistId = artistId;
                album.GenreId = genreId;
                album.Title = StringCleaner.Clean(title)!;
                album.Released = released;
                album.CoverUrl = StringCleaner.RemoveInvalidCharacters(coverUrl);
                album.IsWishListItem = isWishlistItem;
                album.Purchased = purchased;
                album.Price = price;
                album.RetailerId = retailerId;

                // Save the changes
                await Context.SaveChangesAsync();

                // Reload the album to reflect changes in e.g. retailer
                album = await GetAsync(x => x.Id == albumId);
            }

            return album;
        }

        /// <summary>
        /// Delete the album with the specified Id
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int albumId)
        {
            // Find the album record and check it exists
            Album album = await GetAsync(x => x.Id == albumId);
            if (album != null)
            {
                // Delete the associated tracks
                await Factory.Tracks.DeleteAllTracksForAlbumAsync(albumId);

                // Delete the album record and save changes
                Factory.Context.Remove(album);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}