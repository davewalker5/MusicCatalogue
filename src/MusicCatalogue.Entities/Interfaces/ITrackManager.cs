using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ITrackManager
    {
        Task<Track> AddAsync(int albumId, string title, int? number, int? duration);
        Task<Track> GetAsync(Expression<Func<Track, bool>> predicate);
        Task<List<Track>> ListAsync(Expression<Func<Track, bool>> predicate);
    }
}