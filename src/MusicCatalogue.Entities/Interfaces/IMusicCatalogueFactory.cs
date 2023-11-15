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
        IExporter CsvExporter { get; }
        IExporter XlsxExporter { get; }
        IJobStatusManager JobStatuses { get; }
        IWishListBasedReport<GenreStatistics> GenreStatistics { get; }
        IWishListBasedReport<ArtistStatistics> ArtistStatistics { get; }
    }
}