using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IManufacturerManager
    {
        Task<Manufacturer> AddAsync(string name);
        Task DeleteAsync(int id);
        Task<Manufacturer> GetAsync(Expression<Func<Manufacturer, bool>> predicate);
        Task<List<Manufacturer>> ListAsync(Expression<Func<Manufacturer, bool>> predicate);
        Task<Manufacturer?> UpdateAsync(int id, string name);
    }
}