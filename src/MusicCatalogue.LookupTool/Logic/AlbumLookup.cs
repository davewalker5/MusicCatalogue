using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Logic.Api;
using MusicCatalogue.Logic.Api.TheAudioDB;
using MusicCatalogue.Logic.Collection;
using MusicCatalogue.Logic.Database;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class AlbumLookup
    {
        private readonly IMusicLogger _logger;
        private readonly MusicApplicationSettings _settings;
        private readonly IMusicCatalogueFactory _factory;

        public AlbumLookup(IMusicLogger logger, IMusicCatalogueFactory factory, MusicApplicationSettings settings)
        {
            _logger = logger;
            _settings = settings;
            _factory = factory;
        }

        /// <summary>
        /// Lookup an album given the artist name and album title
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        public async Task LookupAlbum(string artistName, string albumTitle)
        {
            // Get the API key and the URLs for the album and track lookup endpoints
            var key = _settings!.ApiServiceKeys.Find(x => x.Service == ApiServiceType.TheAudioDB)!.Key;
            var albumsEndpoint = _settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Albums)!.Url;
            var tracksEndpoint = _settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Tracks)!.Url;

            // Convert the URL into a URI instance that will expose the host name - this is needed
            // to set up the client headers
            var uri = new Uri(albumsEndpoint);

            // Configure an HTTP client
            var client = MusicHttpClient.Instance;
            client.AddHeader("X-RapidAPI-Key", key);
            client.AddHeader("X-RapidAPI-Host", uri.Host);

            // Configure the APIs
            var albumsApi = new TheAudioDBAlbumsApi(_logger, client, albumsEndpoint);
            var tracksApi = new TheAudioDBTracksApi(_logger, client, tracksEndpoint);
            var lookupManager = new AlbumLookupManager(_logger, albumsApi, tracksApi, _factory);

            // Lookup the album and its tracks
            var album = await lookupManager.LookupAlbum(artistName, albumTitle);
            if (album != null)
            {
                // Dump the album details
                Console.WriteLine($"Title: {album.Title}");
                Console.WriteLine($"Artist: {StringCleaner.Clean(artistName)}");
                Console.WriteLine($"Released: {album.Released}");
                Console.WriteLine($"Genre: {album.Genre}");
                Console.WriteLine($"Cover: {album.CoverUrl}");
                Console.WriteLine();

                // Dump the track list
                if ((album.Tracks != null) && (album.Tracks.Count > 0))
                {
                    foreach (var track in album.Tracks)
                    {
                        Console.WriteLine($"{track.Number} : {track.Title}, {track.FormattedDuration()}");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("No tracks found");
                }
            }
            else
            {
                Console.WriteLine("Album details not found");
            }
        }
    }
}
