using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IRetailerManager
    {
        Task<Retailer> AddAsync(
            string name,
            string? address1 = null,
            string? address2 = null,
            string? town = null,
            string? county = null,
            string? postcode = null,
            string? country = null,
            decimal? latitude = null,
            decimal? longitude = null,
            string? website = null);

        Task<Retailer> GetAsync(Expression<Func<Retailer, bool>> predicate);
        Task<List<Retailer>> ListAsync(Expression<Func<Retailer, bool>> predicate);
        Task<Retailer?> UpdateAsync(
            int retailerId,
            string name,
            string? address1 = null,
            string? address2 = null,
            string? town = null,
            string? county = null,
            string? postcode = null,
            string? country = null,
            decimal? latitude = null,
            decimal? longitude = null,
            string? website = null);

        Task DeleteAsync(int retailerId);
    }
}