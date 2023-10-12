using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IStatisticsManager
    {
        Task PopulateArtistStatistics(IEnumerable<Artist> artists);
    }
}