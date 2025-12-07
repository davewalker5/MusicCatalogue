using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IEquipmentTypeManager
    {
        Task<EquipmentType> AddAsync(string name);
        Task DeleteAsync(int id);
        Task<EquipmentType> GetAsync(Expression<Func<EquipmentType, bool>> predicate);
        Task<List<EquipmentType>> ListAsync(Expression<Func<EquipmentType, bool>> predicate);
        Task<EquipmentType?> UpdateAsync(int id, string name);
    }
}