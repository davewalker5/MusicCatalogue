using MusicCatalogue.Entities.Database;
using System.Linq.Expressions;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistManager
    {
        Task<Artist> AddAsync(
            string name,
            int? vibeId = null,
            int energy = 0,
            int intimacy = 0,
            int warmth = 0,
            VocalPresence vocals = VocalPresence.Unknown,
            EnsembleType ensemble = EnsembleType.Unknown);

        Task<Artist> GetAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums);
        Task<List<Artist>> ListAsync(Expression<Func<Artist, bool>> predicate, bool loadAlbums);

        Task<Artist?> UpdateAsync(
            int id,
            string name,
            int? vibeId = null,
            int energy = 0,
            int intimacy = 0,
            int warmth = 0,
            VocalPresence vocals = VocalPresence.Unknown,
            EnsembleType ensemble = EnsembleType.Unknown);

        Task DeleteAsync(int id);
    }
}