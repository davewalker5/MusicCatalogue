using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistMoodManager
    {
        Task<ArtistMood> AddAsync(int artistId, int moodId);
        Task DeleteAsync(int id);
    }
}