using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicCatalogueFactory
    {
        DbContext Context { get; }
        IGenreManager Genres { get; }
        IAlbumManager Albums { get; }
        IArtistManager Artists { get; }
        ITrackManager Tracks { get; }
        IRetailerManager Retailers { get; }
        IUserManager Users { get; }
        IImporter Importer { get; }
        IExporter CatalogueCsvExporter { get; }
        IExporter CatalogueXlsxExporter { get; }
        IJobStatusManager JobStatuses { get; }
        ISearchManager Search { get; }
        IWishListBasedReport<GenreStatistics> GenreStatistics { get; }
        IWishListBasedReport<ArtistStatistics> ArtistStatistics { get; }
        IWishListBasedReport<MonthlySpend> MonthlySpend { get; }
        IWishListBasedReport<RetailerStatistics> RetailerStatistics { get; }
    }
}