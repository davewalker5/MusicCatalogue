using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Database;
using Serilog.Core;
using System.Globalization;
using MusicCatalogue.Entities.Api;

namespace MusicCatalogue.Logic.Collection
{
    public class AlbumLookupManager : IAlbumLookupManager
    {
        private readonly TextInfo _textInfo = new CultureInfo("en-GB", false).TextInfo;

        private readonly IMusicLogger _logger;
        private readonly IAlbumsApi _albumsApi;
        private readonly ITracksApi _tracksApi;
        private readonly IArtistManager _artistManager;
        private readonly IAlbumManager _albumManager;
        private readonly ITrackManager _trackManager;

        public AlbumLookupManager(
            IMusicLogger logger,
            IAlbumsApi albumsApi,
            ITracksApi tracksApi,
            IArtistManager artistManager,
            IAlbumManager albumManager,
            ITrackManager trackManager)
        {
            _logger = logger;
            _albumsApi = albumsApi;
            _tracksApi = tracksApi;
            _artistManager = artistManager;
            _albumManager = albumManager;
            _trackManager = trackManager;
        }

        /// <summary>
        /// Lookup an album given the artist name and album title
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        /// <returns></returns>
        public async Task<Album?> LookupAlbum(string artistName, string albumTitle)
        {
            // Convert the parameters to title case to match the case used to persist data
            artistName = _textInfo.ToTitleCase(artistName);
            albumTitle = _textInfo.ToTitleCase(albumTitle);

            // See if the album details are held locally, first
            Album? album = await LookupAlbumUsingDb(artistName, albumTitle);
            if (album == null)
            {
                // Not held locally so use the API to look up the details
                _logger.LogMessage(Severity.Info, "Album not found locally - using the API to lookup album details");
                album = await LookupAlbumUsingApi(artistName, albumTitle);

                // An album is valid if it isn't null and it has at least 1 track. If not, then it's
                // not valid and shouldn't be persisted to the database
                var numberOfTracks = (album != null) && (album.Tracks != null) ? album.Tracks.Count : 0;
                _logger.LogMessage(Severity.Info, $"Found {numberOfTracks} track(s)");

                if (numberOfTracks > 0)
                {
                    // Got valid details from the API so store them locally
                    album = await StoreAlbumLocally(album!);
                }
                else
                {
                    // Got incomplete album details so return null
                    _logger.LogMessage(Severity.Warning, $"Incomplete album details returned by the API");
                    album = null;
                }
            }

            return album;
        }

        /// <summary>
        /// Store an album locally
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private async Task<Album> StoreAlbumLocally(Album template)
        {
            // Save the artist details, first. As with all the database calls in this method, the
            // logic to prevent duplication of artists is in the management class
            var artist = await _artistManager.AddAsync(template!.Artist!.Name);

            // Save the album details
            var album = await _albumManager.AddAsync(artist.Id, template.Title, template.Released, template.Genre, template.CoverUrl);

            // Save the track details
            foreach (var track in template.Tracks!)
            {
                await _trackManager.AddAsync(album.Id, track.Title, track.Number, track.Duration);
            }

            // Retrieve the newly saved album. This will return an object with the related objects
            // attached and all IDs filled in
            album = await _albumManager.GetAsync(x => x.Id == album.Id);
            return album;
        }

        /// <summary>
        /// Look up an album in the local database
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        /// <returns></returns>
        private async Task<Album?> LookupAlbumUsingDb(string artistName, string albumTitle)
        {
            Album? album = null;

            // Check the artist exists
            _logger.LogMessage(Severity.Info, $"Looking for artist '{artistName}' in the database");
            var artist = await _artistManager.GetAsync(x => x.Name == artistName);
            if (artist != null)
            {
                // Look for an album with the specified title by that artist
                _logger.LogMessage(Severity.Info, $"Looking for album '{albumTitle}' in the database");
                album = await _albumManager.GetAsync(x => (x.ArtistId == artist.Id) && (x.Title == albumTitle));
            }

            return album;
        }

        /// <summary>
        /// Use the API to look up album details
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        /// <returns></returns>
        private async Task<Album?> LookupAlbumUsingApi(string artistName, string albumTitle)
        {
            Album? album = null;

            // Use the API to get the album properties
            var albumProperties = await _albumsApi.LookupAlbum(artistName, albumTitle);
            if (albumProperties != null)
            {
                // Get the track details for the album
                var albumId = int.Parse(albumProperties[ApiProperty.AlbumId]);
                var tracks = await _tracksApi.LookupTracks(albumId);

                // Create an album from the properties
                album = ConvertPropertiesToAlbum(albumProperties);

                // Create a list of tracks from the properties
                var trackList = new List<Track>();
                foreach (var trackProperties in tracks)
                {
                    var track = ConvertPropertiesToTrack(trackProperties);
                    trackList.Add(track);
                }

                // Associate the track list with the album
                album.Tracks = trackList;
            }

            return album;
        }

        /// <summary>
        /// Create an album from a dictionary of album properties
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static Album ConvertPropertiesToAlbum(Dictionary<ApiProperty, string> properties)
        {
            bool releasedIsValid = int.TryParse(GetPropertyValue(properties, ApiProperty.Released), out int released);
            return new Album
            {
                Title = GetPropertyValue(properties, ApiProperty.Title),
                Released = releasedIsValid ? released : null,
                Genre = GetPropertyValue(properties, ApiProperty.Genre),
                CoverUrl = GetPropertyValue(properties, ApiProperty.CoverImageUrl),
                Artist = new Artist
                {
                    Name = GetPropertyValue(properties, ApiProperty.Artist)
                }
            };
        }

        /// <summary>
        /// Create a track from a dictionary of track properties
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static Track ConvertPropertiesToTrack(Dictionary<ApiProperty, string> properties)
        {
            var numberIsValid = int.TryParse(GetPropertyValue(properties, ApiProperty.TrackNumber), out int number);
            var durationIsValid = int.TryParse(GetPropertyValue(properties, ApiProperty.TrackLength), out int duration);
            return new Track
            {
                Title = GetPropertyValue(properties, ApiProperty.Title),
                Number = numberIsValid ? number : null,
                Duration = durationIsValid ? duration : null
            };
        }

        /// <summary>
        /// Extract a value from a properties dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetPropertyValue(Dictionary<ApiProperty, string> properties, ApiProperty key)
        {
            properties.TryGetValue(key, out string? value);
            return value ?? "";
        }
    }
}
