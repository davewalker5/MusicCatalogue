using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumLookupManager
    {
        Task<Album?> LookupAlbum(string artistName, string albumTitle);
    }
}