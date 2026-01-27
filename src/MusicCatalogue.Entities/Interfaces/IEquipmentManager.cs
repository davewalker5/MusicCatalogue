using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IEquipmentManager
    {
        Task<Equipment> AddAsync(int equipmentTypeId, int manufacturerId, string description, string? model, string? serialNumber, bool? isWishListItem, DateTime? purchased, decimal? price, int? retailerId);
        Task DeleteAsync(int id);
        Task<Equipment> GetAsync(Expression<Func<Equipment, bool>> predicate);
        Task<List<Equipment>> ListAsync(Expression<Func<Equipment, bool>> predicate);
        Task<Equipment?> UpdateAsync(int id, int equipmentTypeId, int manufacturerId, string description, string? model, string? serialNumber, bool? isWishListItem, DateTime? purchased, decimal? price, int? retailerId);
    }
}