using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class TrackManager : DatabaseManagerBase, ITrackManager
    {
        internal TrackManager(IMusicCatalogueFactory factory) : base(factory)
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
            => await Context.Tracks
                            .Where(predicate)
                            .OrderBy(x => x.Number)
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
            var clean = StringCleaner.Clean(title)!;
            var track = await GetAsync(a => (a.AlbumId == albumId) && (a.Title == clean));

            if (track == null)
            {
                track = new Track
                {
                    AlbumId = albumId,
                    Title = clean,
                    Number = number,
                    Duration = duration
                };
                await Context.Tracks.AddAsync(track);
                await Context.SaveChangesAsync();
            }

            return track;
        }

        /// <summary>
        /// Delete the tracks associated with an album, given its ID
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int albumId)
        {
            List<Track> tracks = await ListAsync(x => x.AlbumId == albumId);
            if (tracks.Any())
            {
                Context.Tracks.RemoveRange(tracks);
                await Context.SaveChangesAsync();
            }
        }
    }
}