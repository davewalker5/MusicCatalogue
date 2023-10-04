using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;

namespace MusicCatalogue.Logic.Factory
{
    public class MusicCatalogueFactory : IMusicCatalogueFactory
    {
        private readonly Lazy<IArtistManager> _artists;
        private readonly Lazy<IAlbumManager> _albums;
        private readonly Lazy<ITrackManager> _tracks;
        private readonly Lazy<IUserManager> _users;

        public IArtistManager Artists { get { return _artists.Value; } }
        public IAlbumManager Albums { get { return _albums.Value; } }
        public ITrackManager Tracks { get { return _tracks.Value; } }
        public IUserManager Users { get { return _users.Value; } }

        public MusicCatalogueFactory(MusicCatalogueDbContext context)
        {
            _artists = new Lazy<IArtistManager>(() => new ArtistManager(context));
            _albums = new Lazy<IAlbumManager>(() => new AlbumManager(context));
            _tracks = new Lazy<ITrackManager>(() => new TrackManager(context));
            _users = new Lazy<IUserManager>(() => new UserManager(context));
        }
    }
}