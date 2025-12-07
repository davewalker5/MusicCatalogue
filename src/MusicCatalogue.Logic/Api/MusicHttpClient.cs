using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.Api
{
    [ExcludeFromCodeCoverage]
    public sealed class MusicHttpClient : IMusicHttpClient
    {
        private readonly static HttpClient _client = new();
        private static MusicHttpClient? _instance = null;
        private readonly static object _lock = new();

        private MusicHttpClient() { }

        /// <summary>
        /// Return the singleton instance of the client
        /// </summary>
        public static MusicHttpClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new MusicHttpClient();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Clear the request headers
        /// </summary>
        public void ClearHeaders()
            => _client.DefaultRequestHeaders.Clear();

        /// <summary>
        /// Add a request header
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
            => _client.DefaultRequestHeaders.Add(name, value);

        /// <summary>
        /// Send a GET request to the specified URI and return the response
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string uri)
            => await _client.GetAsync(uri);
    }
}
