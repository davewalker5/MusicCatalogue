using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class TrackManager : DatabaseManagerBase, ITrackManager
    {
        internal TrackManager(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Return the first track matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Track> GetAsync(Expression<Func<Track, bool>> predicate)
        {
            List<Track> tracks = await ListAsync(predicate);

#pragma warning disable CS8603
            return tracks.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all tracks matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Track>> ListAsync(Expression<Func<Track, bool>> predicate)
            => await _context.Tracks
                             .Include(x => x.Album)
                             .ThenInclude(y => y.Artist)
                             .Where(predicate)
                             .ToListAsync();

        /// <summary>
        /// Add a track, if it doesn't already exist
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public async Task<Track> AddAsync(int albumId, string title, int? number, int? duration)
        {
            var track = await GetAsync(a => (a.AlbumId == albumId) && (a.Title == title));

            if (track == null)
            {
                track = new Track
                {
                    AlbumId = albumId,
                    Title = _textInfo.ToTitleCase(title),
                    Number = number,
                    Duration = duration
                };
                await _context.Tracks.AddAsync(track);
                await _context.SaveChangesAsync();
            }

            return track;
        }
    }
}