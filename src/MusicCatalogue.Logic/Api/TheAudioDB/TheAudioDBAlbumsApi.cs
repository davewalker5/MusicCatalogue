using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Music;

namespace MusicCatalogue.Logic.Api.TheAudioDB
{
    public class TheAudioDBAlbumsApi : ExternalApiBase, IAlbumsApi
    {
        private readonly string _baseAddress;

        public TheAudioDBAlbumsApi(IMusicLogger logger, IMusicHttpClient client, string url) : base(logger, client)
        {
            _baseAddress = url;
        }

        /// <summary>
        /// Lookup an album given an artist and album title
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<Dictionary<ApiProperty, string>?> LookupAlbum(string artist, string title)
        {
            Logger.LogMessage(Severity.Info, $"Looking up album '{title}' by '{artist}'");
            var escapedArtist = Uri.EscapeDataString(artist);
            var escapedTitle = Uri.EscapeDataString(title);
            var properties = await MakeApiRequest($"?s={escapedArtist}&a={escapedTitle}");
            return properties;
        }

        /// <summary>
        /// Make a call to the album search API and 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private async Task<Dictionary<ApiProperty, string>?> MakeApiRequest(string parameters)
        {
            Dictionary<ApiProperty, string>? properties = null;

            // Make a request for the data from the API
            var url = $"{_baseAddress}{parameters}";
            var node = await SendRequest(url);

            if (node != null)
            {
                try
                {
                    // Extract the response element from the JSON DOM
                    var apiResponse = node!["album"]![0];

                    // Extract the values into a dictionary
                    properties = new()
                    {
                        { ApiProperty.AlbumId, apiResponse!["idAlbum"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.ArtistId, apiResponse!["idArtist"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.Title, apiResponse!["strAlbum"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.Artist, apiResponse!["strArtist"]?.GetValue<string>() ?? "" },
                        { ApiProperty.Released, apiResponse!["intYearReleased"]?.GetValue<string>() ?? "" },
                        { ApiProperty.Genre, apiResponse!["strGenre"]?.GetValue<string>() ?? "" },
                        { ApiProperty.CoverImageUrl, apiResponse!["strAlbumThumb"]?.GetValue<string>() ?? "" }
                    };

                    // Log the properties dictionary
                    LogProperties(properties!);
                }
                catch (Exception ex)
                {
                    var message = $"Error processing response: {ex.Message}";
                    Logger.LogMessage(Severity.Error, message);
                    Logger.LogException(ex);
                    properties = null;
                }
            }

            return properties;
        }
    }
}
