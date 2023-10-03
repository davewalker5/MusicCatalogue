using MusicCatalogue.Entities.Music;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumsApi
    {
        Task<Dictionary<ApiProperty, string>?> LookupAlbum(string artist, string title);
    }
}