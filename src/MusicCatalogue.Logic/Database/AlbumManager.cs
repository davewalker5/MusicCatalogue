using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class AlbumManager : DatabaseManagerBase, IAlbumManager
    {
        public AlbumManager(MusicCatalogueDbContext context) : base(context)
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
            => await _context.Albums
                             .Include(x => x.Artist)
                             .Include(x => x.Tracks)
                             .Where(predicate)
                             .ToListAsync();

        /// <summary>
        /// Add an album, if it doesn't already exist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="title"></param>
        /// <param name="released"></param>
        /// <param name="genre"></param>
        /// <param name="coverUrl"></param>
        /// <returns></returns>
        public async Task<Album> AddAsync(int artistId, string title, int? released, string? genre, string? coverUrl)
        {
            var album = await GetAsync(a => (a.ArtistId == artistId) && (a.Title == title));

            if (album == null)
            {
                album = new Album
                {
                    ArtistId = artistId,
                    Title = _textInfo.ToTitleCase(title),
                    Released = released,
                    Genre = genre,
                    CoverUrl = coverUrl
                };
                await _context.Albums.AddAsync(album);
                await _context.SaveChangesAsync();
            }

            return album;
        }
    }
}