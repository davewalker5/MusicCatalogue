using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.BusinessLogic.Database
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
        public async Task<Track?> GetAsync(Expression<Func<Track, bool>> predicate)
        {
            var tracks = await ListAsync(predicate);
            return tracks.FirstOrDefault();
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
        /// Update an existing track
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="albumId"></param>
        /// <param name="title"></param>
        /// <param name="number"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public async Task<Track?> UpdateAsync(int trackId, int albumId, string title, int? number, int? duration)
        {
            var track = Context.Tracks.FirstOrDefault(x => x.Id == trackId);
            if (track != null)
            {
                track.AlbumId = albumId;
                track.Title = title;
                track.Number = number;
                track.Duration = duration;
                await Context.SaveChangesAsync();
            }

            return track;
        }

        /// <summary>
        /// Delete the track with the specified Id
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int trackId)
        {
            var track = Context.Tracks.FirstOrDefault(x => x.Id == trackId);
            if (track != null)
            {
                Context.Tracks.Remove(track);
                await Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete the tracks associated with an album, given its ID
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task DeleteAllTracksForAlbumAsync(int albumId)
        {
            var tracks = await ListAsync(x => x.AlbumId == albumId);
            if (tracks.Any())
            {
                Context.Tracks.RemoveRange(tracks);
                await Context.SaveChangesAsync();
            }
        }
    }
}