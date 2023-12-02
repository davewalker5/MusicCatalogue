using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ISearchManager
    {
        Task<List<Album>?> AlbumSearchAsync(AlbumSearchCriteria criteria);
        Task<List<Artist>?> ArtistSearchAsync(ArtistSearchCriteria criteria);
        Task<List<Genre>?> GenreSearchAsync(GenreSearchCriteria criteria);
        Task<List<Equipment>?> EquipmentSearchAsync(EquipmentSearchCriteria criteria);
    }
}