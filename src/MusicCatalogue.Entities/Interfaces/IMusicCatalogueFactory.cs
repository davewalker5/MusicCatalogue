namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicCatalogueFactory
    {
        IAlbumManager Albums { get; }
        IArtistManager Artists { get; }
        ITrackManager Tracks { get; }
        IUserManager Users { get; }
        IImporter Importer { get; }
        IExporter CsvExporter { get; }
        IExporter XlsxExporter { get; }
        IStatisticsManager Statistics { get; }
        IJobStatusManager JobStatuses { get; }
    }
}