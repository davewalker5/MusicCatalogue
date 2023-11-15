using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IGenreManager
    {
        Task<Genre> AddAsync(string name);
        Task<Genre> GetAsync(Expression<Func<Genre, bool>> predicate);
        Task<List<Genre>> ListAsync(Expression<Func<Genre, bool>> predicate);
    }
}