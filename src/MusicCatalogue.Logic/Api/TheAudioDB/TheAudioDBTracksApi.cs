using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Music;
using System.Text.Json.Nodes;

namespace MusicCatalogue.Logic.Api.TheAudioDB
{
    public class TheAudioDBTracksApi : ExternalApiBase, ITracksApi
    {
        private readonly string _baseAddress;

        public TheAudioDBTracksApi(IMusicLogger logger, IMusicHttpClient client, string url) : base(logger, client)
        {
            _baseAddress = url;
        }

        /// <summary>
        /// Lookup an album given an artist and album title
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<List<Dictionary<ApiProperty, string>>> LookupTracks(int albumId)
        {
            Logger.LogMessage(Severity.Info, $"Looking up the tracks for the album with Id '{albumId}'");
            var properties = await MakeApiRequest($"?m={albumId}");
            return properties;
        }

        /// <summary>
        /// Make a call to the track search API and extract the track details
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private async Task<List<Dictionary<ApiProperty, string>>> MakeApiRequest(string parameters)
        {
            List<Dictionary<ApiProperty, string>> properties = new();

            // Make a request for the data from the API
            var url = $"{_baseAddress}{parameters}";
            var node = await SendRequest(url);

            if (node != null)
            {
                // The response should contain a JSON array of track nodes. Iterate over
                // them
                JsonArray? tracks = node!["track"] as JsonArray;
                if (tracks != null)
                {
                    foreach (var track in tracks!)
                    {
                        // Extract the properties of this track
                        var trackProperties = GetTrackPropertiesFromNode(track!);
                        if (trackProperties != null)
                        {
                            properties.Add(trackProperties);
                        }
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Extract a set of track properties from a JSON DOM node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Dictionary<ApiProperty, string>? GetTrackPropertiesFromNode(JsonNode node)
        {
            Dictionary<ApiProperty, string>? properties;

            try
            {
                // Extract the property values
                properties = new()
                    {
                        { ApiProperty.TrackId, node!["idTrack"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.Title, node!["strTrack"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.TrackLength, node!["intDuration"]?.GetValue<string?>() ?? "" },
                        { ApiProperty.TrackNumber, node!["intTrackNumber"]?.GetValue<string>() ?? "" }
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

            return properties;
        }
    }
}
