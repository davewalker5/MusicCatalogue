using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistManager
    {
        Task<Artist> AddAsync(string name);
        Task<Artist> GetAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums);
        Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums);
        Task<Artist?> UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }
}