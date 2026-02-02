using MusicCatalogue.Entities.Api;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.BusinessLogic.Database;

namespace MusicCatalogue.BusinessLogic.Collection
{
    public class AlbumLookupManager : IAlbumLookupManager
    {
        private readonly IAlbumsApi _albumsApi;
        private readonly ITracksApi _tracksApi;
        private readonly IMusicCatalogueFactory _factory;

        public AlbumLookupManager(
            IAlbumsApi albumsApi,
            ITracksApi tracksApi,
            IMusicCatalogueFactory factory)
        {
            _albumsApi = albumsApi;
            _tracksApi = tracksApi;
            _factory = factory;
        }

        /// <summary>
        /// Lookup an album given the artist name and album title
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        /// <param name="storeAlbumInWishlist"></param>
        /// <returns></returns>
        public async Task<Album?> LookupAlbum(string artistName, string albumTitle, bool storeAlbumInWishlist)
        {
            // Convert the parameters to title case to match the case used to persist data
            artistName = StringCleaner.Clean(artistName)!;
            albumTitle = StringCleaner.Clean(albumTitle)!;

            // See if the album details are held locally, first
            Album? album = await LookupAlbumUsingDb(artistName!, albumTitle!);
            if (album == null)
            {
                // Not held locally so use the API to look up the details
                _factory.Logger.LogMessage(Severity.Info, "Album not found locally - using the API to lookup album details");
                album = await LookupAlbumUsingApi(artistName!, albumTitle!);

                // An album is valid if it isn't null and it has at least 1 track. If not, then it's
                // not valid and shouldn't be persisted to the database
                var numberOfTracks = (album != null) && (album.Tracks != null) ? album.Tracks.Count : 0;
                _factory.Logger.LogMessage(Severity.Info, $"Found {numberOfTracks} track(s)");

                if (numberOfTracks > 0)
                {
                    // Got valid details from the API so store them locally
                    album = await StoreAlbumLocally(artistName!, album!, storeAlbumInWishlist);
                }
                else
                {
                    // Got incomplete album details so return null
                    _factory.Logger.LogMessage(Severity.Warning, $"Incomplete album details returned by the API");
                    album = null;
                }
            }

            return album;
        }

        /// <summary>
        /// Store an album locally
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="template"></param>
        /// <param name="storeAlbumInWishlist"></param>
        /// <returns></returns>
        private async Task<Album> StoreAlbumLocally(string artistName, Album template, bool storeAlbumInWishlist)
        {
            // Save the artist details, first. As with all the database calls in this method, the
            // logic to prevent duplication of artistsand genres is in the management class
            var artist = await _factory.Artists.AddAsync(artistName);
            var genre = await _factory.Genres.AddAsync(template.Genre!.Name, false);

            // Save the album details
            var album = await _factory.Albums.AddAsync(
                artist.Id,
                genre.Id,
                template.Title, 
                template.Released,
                template.CoverUrl,
                storeAlbumInWishlist,
                template.Purchased,
                template.Price,
                template.RetailerId);

            // Save the track details
            foreach (var track in template.Tracks!)
            {
                await _factory.Tracks.AddAsync(album.Id, track.Title, track.Number, track.Duration);
            }

            // Retrieve the newly saved album. This will return an object with the related objects
            // attached and all IDs filled in
            album = await _factory.Albums.GetAsync(x => x.Id == album.Id);
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
            _factory.Logger.LogMessage(Severity.Info, $"Looking for artist '{artistName}' in the database");
            var artist = await _factory.Artists.GetAsync(x => x.Name == artistName, false);
            if (artist != null)
            {
                // Look for an album with the specified title by that artist
                _factory.Logger.LogMessage(Severity.Info, $"Looking for album '{albumTitle}' in the database");
                album = await _factory.Albums.GetAsync(x => (x.ArtistId == artist.Id) && (x.Title == albumTitle));
                if (album != null)
                {
                    _factory.Logger.LogMessage(Severity.Info, $"Album '{album.Id} - {album.Title}' found locally");
                }
                else
                {
                    _factory.Logger.LogMessage(Severity.Info, $"Album '{albumTitle}' not found locally");

                }
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
                Genre = new Genre
                {
                    Name = GetPropertyValue(properties, ApiProperty.Genre)
                },
                CoverUrl = GetPropertyValue(properties, ApiProperty.CoverImageUrl)
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
