using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IVibeManager
    {
        Task<Vibe> AddAsync(string name);
        Task DeleteAsync(int id);
        Task<Vibe> GetAsync(Expression<Func<Vibe, bool>> predicate);
        Task<List<Vibe>> ListAsync(Expression<Func<Vibe, bool>> predicate);
        Task<Vibe?> UpdateAsync(int id, string name);
    }
}