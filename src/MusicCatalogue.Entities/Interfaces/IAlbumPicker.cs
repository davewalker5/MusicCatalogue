using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumPicker
    {
        Task<List<PickedAlbum>> PickAsync(AlbumSelectionCriteria criteria);
    }
}