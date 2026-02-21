using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;
using System.Linq.Expressions;

namespace MusicCatalogue.BusinessLogic.Database
{
    public class SessionManager : DatabaseManagerBase, ISessionManager
    {
        internal SessionManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first session matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Session> GetAsync(Expression<Func<Session, bool>> predicate)
        {
            List<Session> sessions = await ListAsync(predicate, 1, 1);

#pragma warning disable CS8603
            return sessions.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return sessions matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<Session>> ListAsync(Expression<Func<Session, bool>> predicate, int pageNumber, int pageSize)
            => await Context.Sessions
                            .Include(s => s.SessionAlbums)
                                .ThenInclude(sa => sa.Album!)
                                    .ThenInclude(a => a.Genre)
                            .Include(s => s.SessionAlbums)
                                .ThenInclude(sa => sa.Album!)
                                    .ThenInclude(a => a.Retailer)
                            .Include(s => s.SessionAlbums)
                                .ThenInclude(sa => sa.Album!)
                                    .ThenInclude(a => a.Artist)
                            .Include(s => s.SessionAlbums)
                                .ThenInclude(sa => sa.Album!)
                                    .ThenInclude(a => a.Tracks)
                            .Where(predicate)
                            .OrderBy(x => x.CreatedAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        /// <summary>
        /// Add a new session
        /// </summary>
        /// <param name="createdAt"></param>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <returns></returns>
        public async Task<Session> AddAsync(DateTime createdAt, PlaylistType type, TimeOfDay timeOfDay, IEnumerable<int> albumIds)
        {
            // Create the session
            var session = new Session
            {
                CreatedAt = createdAt,
                Type = type,
                TimeOfDay = timeOfDay
            };

            await Context.Sessions.AddAsync(session);
            await Context.SaveChangesAsync();

            // Add the albums to it, in order
            var sessionAlbums = albumIds
                .Select((albumId, position) => new SessionAlbum
                {
                    SessionId = session.Id,
                    AlbumId   = albumId,
                    Position  = position + 1
                });

            await Context.SessionAlbums.AddRangeAsync(sessionAlbums);
            await Context.SaveChangesAsync();

            // Load it to load the related entities
            session = await GetAsync(x => x.Id == session.Id);
            return session;
        }

        /// <summary>
        /// Delete the session with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the session record and check it exists
            var session = await GetAsync(x => x.Id == id);
            if (session != null)
            {
                // Identify any associated albums and remove the link
                var sessionAlbums = Context.SessionAlbums.Where(x => x.SessionId == id);
                Context.RemoveRange(sessionAlbums);

                // Delete the session record and save changes
                Factory.Context.Remove(session);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}