using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IAlbumPicker
    {
        Task<List<PickedAlbum>> PickAsync(AlbumSelectionCriteria criteria);
    }
}