using MusicCatalogue.Data;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.Api;
using MusicCatalogue.Logic.Api.TheAudioDB;
using MusicCatalogue.Logic.Collection;
using MusicCatalogue.Logic.Config;
using MusicCatalogue.Logic.Factory;
using MusicCatalogue.Logic.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace MusicCatalogue.LookupPoC
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Check the arguments are OK
            if (args.Length != 2)
            {
                Console.WriteLine($"Usage: {System.AppDomain.CurrentDomain.FriendlyName} \"artist\" \"album title\"");
                return;
            }

            // Read the application config file
            var settings = new ConfigReader<MusicApplicationSettings>().Read("appsettings.json");

            // Configure the log file
            var logger = new FileLogger();
            logger.Initialise(settings!.LogFile, settings.MinimumLogLevel);

            // Get the version number and application title
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = $"Music Catalogue Lookup Tool v{info.FileVersion}";

            // Log the startup messages
            logger.LogMessage(Severity.Info, new string('=', 80));
            logger.LogMessage(Severity.Info, title);

            // Get the API key and the URLs for the album and track lookup endpoints
            var key = settings.ApiServiceKeys.Find(x => x.Service == ApiServiceType.TheAudioDB)!.Key;
            var albumsEndpoint = settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Albums)!.Url;
            var tracksEndpoint = settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Tracks)!.Url;

            // Convert the URL into a URI instance that will expose the host name - this is needed
            // to set up the client headers
            var uri = new Uri(albumsEndpoint);

            // Configure an HTTP client
            var client = MusicHttpClient.Instance;
            client.AddHeader("X-RapidAPI-Key", key);
            client.AddHeader("X-RapidAPI-Host", uri.Host);

            // Configure the database management classes
            var context = new MusicCatalogueDbContextFactory().CreateDbContext(Array.Empty<string>());
            var factory = new MusicCatalogueFactory(context);

            // Configure the APIs
            var albumsApi = new TheAudioDBAlbumsApi(logger, client, albumsEndpoint);
            var tracksApi = new TheAudioDBTracksApi(logger, client, tracksEndpoint);
            var lookupManager = new AlbumLookupManager(logger, albumsApi, tracksApi, factory);

            // Lookup the album and its tracks
            var album = await lookupManager.LookupAlbum(args[0], args[1]);
            if (album != null)
            {
                // Convert the artist name to title case for display
                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                var artistName = textInfo.ToTitleCase(args[0]);

                // Dump the album details
                Console.WriteLine($"Title: {album.Title}");
                Console.WriteLine($"Artist: {artistName}");
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