using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Factory;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class AlbumManager : IAlbumManager
    {
        private readonly MusicCatalogueFactory _factory;
        private readonly MusicCatalogueDbContext? _context;

        internal AlbumManager(MusicCatalogueFactory factory)
        {
            _factory = factory;
            _context = factory.Context as MusicCatalogueDbContext;
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
            => await _context!.Albums
                              .Where(predicate)
                              .OrderBy(x => x.Title)
                              .Include(x => x.Retailer)
                              .Include(x => x.Tracks)
                              .ToListAsync();

        /// <summary>
        /// Add an album, if it doesn't already exist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="title"></param>
        /// <param name="released"></param>
        /// <param name="genre"></param>
        /// <param name="coverUrl"></param>
        /// <param name="isWishlistItem"></param>
        /// <returns></returns>
        public async Task<Album> AddAsync(int artistId, string title, int? released, string? genre, string? coverUrl, bool? isWishlistItem)
        {
            var clean = StringCleaner.Clean(title)!;
            var album = await GetAsync(a => (a.ArtistId == artistId) && (a.Title == clean));

            if (album == null)
            {
                album = new Album
                {
                    ArtistId = artistId,
                    Title = clean,
                    Released = released,
                    Genre = StringCleaner.RemoveInvalidCharacters(genre),
                    CoverUrl = StringCleaner.RemoveInvalidCharacters(coverUrl),
                    IsWishListItem = isWishlistItem
                };
                await _context!.Albums.AddAsync(album);
                await _context.SaveChangesAsync();
            }

            return album;
        }

        /// <summary>
        /// Update the properties of the specified album
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="artistId"></param>
        /// <param name="title"></param>
        /// <param name="released"></param>
        /// <param name="genre"></param>
        /// <param name="coverUrl"></param>
        /// <param name="isWishlistItem"></param>
        /// <returns></returns>
        public async Task<Album?> UpdateAsync(int albumId, int artistId, string title, int? released, string? genre, string? coverUrl, bool? isWishlistItem)
        {
            var album = await GetAsync(x => x.Id == albumId);
            if (album != null)
            {
                album.ArtistId = artistId;
                album.Title = StringCleaner.Clean(title)!;
                album.Released = released;
                album.Genre = StringCleaner.RemoveInvalidCharacters(genre);
                album.CoverUrl = StringCleaner.RemoveInvalidCharacters(coverUrl);
                album.IsWishListItem = isWishlistItem;

                await _context!.SaveChangesAsync();
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
                await _factory.Tracks.DeleteAsync(albumId);

                // Delete the album record and save changes
                _factory.Context.Remove(album);
                await _factory.Context.SaveChangesAsync();
            }
        }
    }
}