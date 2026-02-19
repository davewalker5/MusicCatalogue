using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Playlists;
using MusicCatalogue.Entities.Search;

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
        public async Task<ActionResult<Session>> SaveSessionAsync([FromBody] SessionTemplate template)
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

        /// <summary>
        /// Return a list of sessions matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<List<Session>>> ListSavedSessionsAsync([FromBody] SessionSearchCriteria criteria)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Searching for saved sessions matching criteria {criteria}");

            // Build the filtering criteria
            Expression<Func<Session, bool>> predicate = s =>
                (!criteria.From.HasValue || s.CreatedAt >= criteria.From.Value) &&
                (!criteria.To.HasValue   || s.CreatedAt <= criteria.To.Value) &&
                (!criteria.Type.HasValue || s.Type == criteria.Type.Value) &&
                (!criteria.TimeOfDay.HasValue || s.TimeOfDay == criteria.TimeOfDay.Value);

            // Get a list of matching sessions
            var sessions = await _factory.SessionManager.ListAsync(predicate, criteria.PageNumber, criteria.PageSize);

            if (sessions.Count == 0)
            {
                _factory.Logger.LogMessage(Severity.Error, $"No matching sessions found");
                return NoContent();
            }

            return sessions;
        }
    }
}