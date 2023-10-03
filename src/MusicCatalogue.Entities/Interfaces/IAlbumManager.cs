using MusicCatalogue.Entities.Music;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumManager
    {
        Task<Album> AddAsync(int artistId, string title, int? released, string? genre, string? coverUrl);
        Task<Album> GetAsync(Expression<Func<Album, bool>> predicate);
        Task<List<Album>> ListAsync(Expression<Func<Album, bool>> predicate);
    }
}