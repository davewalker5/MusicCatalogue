using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IRetailerManager
    {
        Task<Retailer> AddAsync(string name);
        Task<Retailer> GetAsync(Expression<Func<Retailer, bool>> predicate);
        Task<List<Retailer>> ListAsync(Expression<Func<Retailer, bool>> predicate);
        Task<Retailer?> UpdateAsync(int retailerId, string name);
        Task DeleteAsync(int retailerId);
    }
}