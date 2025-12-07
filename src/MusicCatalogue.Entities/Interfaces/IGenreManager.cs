using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IGenreManager
    {
        Task<Genre> AddAsync(string name, bool raiseErrorIfExists);
        Task<Genre> GetAsync(Expression<Func<Genre, bool>> predicate);
        Task<List<Genre>> ListAsync(Expression<Func<Genre, bool>> predicate);
        Task<Genre?> UpdateAsync(int genreId, string name);
        Task DeleteAsync(int genreId);
    }
}