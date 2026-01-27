using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.BusinessLogic.Api.TheAudioDB;
using MusicCatalogue.BusinessLogic.Collection;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Tests.Mocks;

namespace MusicCatalogue.Tests
{
    /// <summary>
    /// These tests can't test authentication/authorisation at the service end, the lookup of data at the
    /// service end or network transport. They're design to test the downstream logic once a response has
    /// been received
    /// </summary>
    [TestClass]
    public class AlbumLookupManagerTest
    {
        private const string ArtistName = "John Coltrane";
        private const string AlbumTitle = "Blue Train";
        private const int Released = 1957;
        private const string Genre = "Jazz";
        private const string CoverUrl = "blue-train-4e43eba6d7b16.jpg";

        private const string AlbumNotFoundResponse = "{\"album\": null}";
        private const string AlbumResponse = "{\"album\": [{\"idAlbum\": \"2132261\",\"idArtist\": \"114605\",\"strAlbum\": \"Blue Train\",\"strArtist\": \"John Coltrane\",\"intYearReleased\": \"1957\",\"strGenre\": \"Jazz\",\"strAlbumThumb\": \"blue-train-4e43eba6d7b16.jpg\"}]}";
        private const string TracksNotFoundResponse = "{\"track\": null}";
        private const string MalformedTracksResponse = "{\"track\": \"Hello\"}";
        private const string TracksResponse = "{\"track\": [{\"idTrack\": \"32996716\",\"strTrack\": \"Blue Train\",\"strAlbum\": \"Blue Train\",\"intDuration\": \"643200\",\"intTrackNumber\": \"1\"}]}";

        private MockHttpClient? _client = null;
        private IAlbumLookupManager? _manager = null;
        private IMusicCatalogueFactory? _factory = null;

        [TestInitialize]
        public void Initialise()
        {
            // Create the logger, mock client and API wrappers
            var logger = new MockFileLogger();
            _client = new MockHttpClient();
            var albumsApi = new TheAudioDBAlbumsApi(logger, _client, "");
            var tracksApi = new TheAudioDBTracksApi(logger, _client, "");

            // Create the database management classes
            var context = MusicCatalogueDbContextFactory.CreateInMemoryDbContext();
            _factory = new MusicCatalogueFactory(context);

            // Create a lookup manager
            _manager = new AlbumLookupManager(logger, albumsApi, tracksApi, _factory);
        }

        [TestMethod]
        public void AlbumNotFoundTest()
        {
            _client!.AddResponse(AlbumNotFoundResponse);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;
            Assert.IsNull(album);
        }

        [TestMethod]
        public void AlbumRequestNetworkErrorTest()
        {
            _client!.AddResponse(null);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;
            Assert.IsNull(album);
        }

        [TestMethod]
        public void TracksNotFoundTest()
        {
            _client!.AddResponse(AlbumResponse);
            _client.AddResponse(TracksNotFoundResponse);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;

            Assert.IsNull(album);
        }

        [TestMethod]
        public void MalformedTracksResponseTest()
        {
            _client!.AddResponse(AlbumResponse);
            _client!.AddResponse(MalformedTracksResponse);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;

            Assert.IsNull(album);
        }

        [TestMethod]
        public void AlbumWithTracksTest()
        {
            _client!.AddResponse(AlbumResponse);
            _client.AddResponse(TracksResponse);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;

            Assert.IsNotNull(album);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre!.Name);
            Assert.AreEqual(CoverUrl, album.CoverUrl);

            Assert.IsNotNull(album.Tracks);
            Assert.AreEqual(1, album.Tracks.Count);
            Assert.AreEqual("Blue Train", album.Tracks.First().Title);
            Assert.AreEqual(1, album.Tracks.First().Number);
            Assert.AreEqual(643200, album.Tracks.First().Duration);
            Assert.AreEqual("10:43", album.Tracks.First().FormattedDuration);
        }

        [TestMethod]
        public void ArtistInDbButAlbumNotInDbTest()
        {
            Task.Run(() => _factory!.Artists.AddAsync(ArtistName)).Wait();

            _client!.AddResponse(AlbumResponse);
            _client.AddResponse(TracksResponse);
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;

            Assert.IsNotNull(album);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre!.Name);
            Assert.AreEqual(CoverUrl, album.CoverUrl);

            Assert.IsNotNull(album.Tracks);
            Assert.AreEqual(1, album.Tracks.Count);
            Assert.AreEqual("Blue Train", album.Tracks.First().Title);
            Assert.AreEqual(1, album.Tracks.First().Number);
            Assert.AreEqual(643200, album.Tracks.First().Duration);
            Assert.AreEqual("10:43", album.Tracks.First().FormattedDuration);
        }

        [TestMethod]
        public void ArtistAndAlbumInDbTest()
        {
            var artistId = Task.Run(() => _factory!.Artists.AddAsync(ArtistName)).Result.Id;
            var genreId = Task.Run(() => _factory!.Genres.AddAsync(Genre, false)).Result.Id;
            Task.Run(() => _factory!.Albums.AddAsync(artistId, genreId, AlbumTitle, Released, CoverUrl, false, null, null, null)).Wait();
            var album = Task.Run(() => _manager!.LookupAlbum(ArtistName, AlbumTitle, false)).Result;

            Assert.IsNotNull(album);
            Assert.AreEqual(AlbumTitle, album.Title);
            Assert.AreEqual(Released, album.Released);
            Assert.AreEqual(Genre, album.Genre!.Name);
            Assert.AreEqual(CoverUrl, album.CoverUrl);

            Assert.IsNotNull(album.Tracks);
            Assert.AreEqual(0, album.Tracks.Count);
        }
    }
}
