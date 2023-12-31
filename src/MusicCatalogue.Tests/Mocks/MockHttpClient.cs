﻿using MusicCatalogue.Entities.Interfaces;
using System.Net;

namespace MusicCatalogue.Tests.Mocks
{
    internal class MockHttpClient : IMusicHttpClient
    {
        private readonly Queue<string?> _responses = new();

        /// <summary>
        /// Queue a response
        /// </summary>
        /// <param name="response"></param>
        public void AddResponse(string? response)
        {
            _responses.Enqueue(response);
        }

        /// <summary>
        /// Queue a set of responses
        /// </summary>
        /// <param name="responses"></param>
        public void AddResponses(IEnumerable<string?> responses)
        {
            foreach (string? response in responses)
            {
                _responses.Enqueue(response);
            }
        }

        /// <summary>
        /// Construct and return the next response
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
#pragma warning disable CS1998
        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            // De-queue the next message
            var content = _responses.Dequeue();

            // If the content is null, raise an exception to test the exception handling
            if (content == null)
            {
                throw new Exception();
            }

            // Construct an HTTP response
            var response = new HttpResponseMessage
            {
                Content = new StringContent(content ?? ""),
                StatusCode = HttpStatusCode.OK
            };

            return response;
        }
#pragma warning restore CS1998

        /// <summary>
        /// Clear the request headers
        /// </summary>
        public void ClearHeaders()
        {
        }

        /// <summary>
        /// Add an HTTP request header
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
        }
    }
}
