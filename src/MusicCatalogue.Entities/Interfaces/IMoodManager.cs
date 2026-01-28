using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMoodManager
    {
        Task<Mood> AddAsync(string name);
        Task DeleteAsync(int id);
        Task<Mood> GetAsync(Expression<Func<Mood, bool>> predicate);
        Task<List<Mood>> ListAsync(Expression<Func<Mood, bool>> predicate);
        Task<Mood?> UpdateAsync(int id, string name);
    }
}