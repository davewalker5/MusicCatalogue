using MusicCatalogue.Entities.Music;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistManager
    {
        Task<Artist> AddAsync(string name);
        Task<Artist> GetAsync(Expression<Func<Artist, bool>> predicate);
        Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate);
    }
}