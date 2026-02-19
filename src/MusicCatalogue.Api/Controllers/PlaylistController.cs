using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class PlaylistController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public PlaylistController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Generate a playlist using the specified playlist builder criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generate")]
        public async Task<ActionResult<Playlist>> GeneratePlaylistAsync([FromBody] PlaylistBuilderCriteria criteria)
        {
            // Generate the playlist
            _factory.Logger.LogMessage(Severity.Debug, $"Generating a playlist using criteria {criteria}");
            var playlist = await _factory.PlaylistBuilder.BuildPlaylistAsync(
                criteria.Type,
                criteria.TimeOfDay,
                criteria.CurrentArtistId,
                criteria.NumberOfEntries,
                criteria.IncludedGenreIds,
                criteria.ExcludedGenreIds);

            return playlist;
        }

        /// <summary>
        /// Save a session or generated playlist
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<Session>> GeneratePlaylistAsync([FromBody] SessionTemplate template)
        {
            // Save the session
            _factory.Logger.LogMessage(Severity.Debug, $"Saving session {template}");
            var session = await _factory.SessionManager.AddAsync(
                DateTime.Now,
                template.Type,
                template.TimeOfDay,
                template.AlbumIds);

            return session;
        }
    }
}