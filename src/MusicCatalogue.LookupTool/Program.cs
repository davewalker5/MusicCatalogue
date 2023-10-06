using MusicCatalogue.Data;
using MusicCatalogue.Entities.CommandLine;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.Api;
using MusicCatalogue.Logic.Api.TheAudioDB;
using MusicCatalogue.Logic.Collection;
using MusicCatalogue.Logic.CommandLine;
using MusicCatalogue.Logic.Config;
using MusicCatalogue.Logic.Database;
using MusicCatalogue.Logic.Factory;
using MusicCatalogue.Logic.Logging;
using System.Diagnostics;
using System.Reflection;

namespace MusicCatalogue.LookupPoC
{
    public static class Program
    {
        private static CommandLineParser? _parser= null;
        private static MusicApplicationSettings? _settings = null;
        private static FileLogger? _logger = null;
        private static MusicCatalogueFactory? _factory = null;

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            // Parse the command line
            _parser = new();
            _parser.Add(CommandLineOptionType.Lookup, "--lookup", "-l", "Lookup an album and display its details", 2, 2);
            _parser.Parse(args);

            // Read the application settings
            _settings = new MusicCatalogueConfigReader().Read("appsettings.json");

            // Configure the log file
            _logger = new FileLogger();
            _logger.Initialise(_settings!.LogFile, _settings.MinimumLogLevel);

            // Get the version number and application title
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = $"Music Catalogue Lookup Tool v{info.FileVersion}";

            // Log the startup messages
            Console.WriteLine($"{title}\n");
            _logger.LogMessage(Severity.Info, new string('=', 80));
            _logger.LogMessage(Severity.Info, title);

            // Configure the business logic factory
            var context = new MusicCatalogueDbContextFactory().CreateDbContext(Array.Empty<string>());
            _factory = new MusicCatalogueFactory(context);

            // If this is a lookup, look up the album details
            var values = _parser.GetValues(CommandLineOptionType.Lookup);
            if (values != null)
            {
                await LookupAlbum(values[0], values[1]);
            }
        }

        /// <summary>
        /// Lookup an album given the artist name and album title
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        private static async Task LookupAlbum(string artistName, string albumTitle)
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