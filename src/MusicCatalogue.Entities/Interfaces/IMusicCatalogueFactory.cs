namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicCatalogueFactory
    {
        IAlbumManager Albums { get; }
        IArtistManager Artists { get; }
        ITrackManager Tracks { get; }
        IUserManager Users { get; }
    }
}