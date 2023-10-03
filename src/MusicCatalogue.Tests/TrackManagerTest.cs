using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Database;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class TrackManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "https://some.host/blue-train.jpg";
        private const string TrackTitle = "Blue Train";
        private const int TrackNumber = 1;
        private const int TrackDuration = 643200;

        private ITrackManager? _manager = null;
        private int _artistId;
        private int _albumId;

        [TestInitialize]
        public void TestInitialize()
        {
            MusicCatalogueDbContext context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();

            // Set up an artist and album for the tracks to belong to
            _artistId = Task.Run(() => new ArtistManager(context).AddAsync(ArtistName)).Result.Id;
            _albumId = Task.Run(() => new AlbumManager(context).AddAsync(_artistId, AlbumTitle, Released, Genre, CoverUrl)).Result.Id;

            // Create a track manager and add a test track
            _manager = new TrackManager(context);
            Task.Run(() => _manager.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration)).Wait();
        }

        [TestMethod]
        public async Task AddDuplicateTest()
        {
            await _manager!.AddAsync(_albumId, TrackTitle, TrackNumber, TrackDuration);
            var tracks = await _manager.ListAsync(x => true);
            Assert.AreEqual(1, tracks.Count);
        }

        [TestMethod]
        public async Task AddAndGetTest()
        {
            var track = await _manager!.GetAsync(a => a.Title == TrackTitle);
            Assert.IsNotNull(track);
            Assert.IsTrue(track.Id > 0);
            Assert.AreEqual(_albumId, track.AlbumId);
            Assert.AreEqual(TrackTitle, track.Title);
            Assert.AreEqual(TrackNumber, track.Number);
            Assert.AreEqual(TrackDuration, track.Duration);

            Assert.IsNotNull(track.Album);
            Assert.AreEqual(AlbumTitle, track.Album.Title);

            Assert.IsNotNull(track.Album.Artist);
            Assert.AreEqual(ArtistName, track.Album.Artist.Name);
        }

        [TestMethod]
        public async Task GetMissingTest()
        {
            var track = await _manager!.GetAsync(a => a.Title == "Missing");
            Assert.IsNull(track);
        }

        [TestMethod]
        public async Task ListAllTest()
        {
            var tracks = await _manager!.ListAsync(x => true);
            Assert.AreEqual(1, tracks!.Count);
            Assert.AreEqual(TrackTitle, tracks.First().Title);
        }

        [TestMethod]
        public async Task ListMissingTest()
        {
            var tracks = await _manager!.ListAsync(e => e.Title == "Missing");
            Assert.AreEqual(0, tracks!.Count);
        }
    }
}
