using System.Linq.Expressions;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ISessionManager
    {
        Task<Session> GetAsync(Expression<Func<Session, bool>> predicate);
        Task<List<Session>> ListAsync(Expression<Func<Session, bool>> predicate, int pageNumber, int pageSize);
        Task<Session> AddAsync(DateTime createdAt, PlaylistType type, TimeOfDay timeOfDay, IEnumerable<int> albumIds);
        Task DeleteAsync(int id);
    }
}