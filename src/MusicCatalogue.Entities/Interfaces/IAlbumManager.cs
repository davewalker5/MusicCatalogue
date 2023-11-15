using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumManager
    {
        Task<Album> AddAsync(
            int artistId,
            int genreId,
            string title,
            int? released,
            string? coverUrl,
            bool? isWishlistItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId);

        Task<Album?> UpdateAsync(
            int albumId,
            int artistId,
            int genreId,
            string title,
            int? released,
            string? coverUrl,
            bool? isWishlistItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId);

        Task<Album> GetAsync(Expression<Func<Album, bool>> predicate);
        Task<List<Album>> ListAsync(Expression<Func<Album, bool>> predicate);
        Task DeleteAsync(int albumId);
    }
}