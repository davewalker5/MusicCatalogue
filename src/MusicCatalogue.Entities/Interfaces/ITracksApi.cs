using MusicCatalogue.Entities.Music;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ITracksApi
    {
        Task<List<Dictionary<ApiProperty, string>>> LookupTracks(int albumId);
    }
}