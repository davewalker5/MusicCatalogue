using MusicCatalogue.Entities.Music;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumLookupManager
    {
        Task<Album?> LookupAlbum(string artist, string title);
    }
}