﻿using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Music;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace MusicCatalogue.Logic.Api
{
    public abstract class ExternalApiBase
    {
        protected IMusicHttpClient Client { get; private set; }

        protected IMusicLogger Logger { get; private set; }

        protected ExternalApiBase(IMusicLogger logger, IMusicHttpClient client)
        {
            Logger = logger;
            Client = client;
        }

        /// <summary>
        /// Make a request to the specified URL and return the response properties as a JSON DOM
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        protected virtual async Task<JsonNode?> SendRequest(string endpoint)
        {
            JsonNode? node = null;

            try
            {
                // Make a request for the data from the API
                using (var response = await Client.GetAsync(endpoint))
                {
                    // Check the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response, parse to a JSON DOM
                        var json = await response.Content.ReadAsStringAsync();
                        node = JsonNode.Parse(json);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = $"Error calling {endpoint}: {ex.Message}";
                Logger.LogMessage(Severity.Error, message);
                Logger.LogException(ex);
                node = null;
            }

            return node;
        }

        /// <summary>
        /// Log the content of a properties dictionary resulting from an external API call
        /// </summary>
        /// <param name="properties"></param>
        [ExcludeFromCodeCoverage]
        protected void LogProperties(Dictionary<ApiProperty, string?>? properties)
        {
            // Check the properties dictionary isn't NULL
            if (properties != null)
            {
                // Not a NULL dictionary, so iterate over all the properties it contains
                foreach (var property in properties)
                {
                    // Construct a message containing the property name and the value, replacing
                    // null values with "NULL"
                    var value = property.Value != null ? property.Value.ToString() : "NULL";
                    var message = $"API property {property.Key.ToString()} = {value}";

                    // Log the message for this property
                    Logger.LogMessage(Severity.Info, message);
                }
            }
            else
            {
                // Log the fact that the properties dictionary is NULL
                Logger.LogMessage(Severity.Warning, "API lookup generated a NULL properties dictionary");
            }
        }
    }
}
