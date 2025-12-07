using MusicCatalogue.Entities.Api;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumsApi
    {
        Task<Dictionary<ApiProperty, string>?> LookupAlbum(string artist, string title);
    }
}