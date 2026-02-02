using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class TracksController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;


        public TracksController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Add a track to the catalogue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Track>> AddTrackAsync([FromBody] Track template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Adding track {template}");
            var track = await _factory.Tracks.AddAsync(template.AlbumId, template.Title, template.Number, template.Duration);
            return track;
        }

        /// <summary>
        /// Update an existing track
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Track?>> UpdateTrackAsync([FromBody] Track template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Updating track {template}");
            var track = await _factory.Tracks.UpdateAsync(
                template.Id,
                template.AlbumId,
                template.Title,
                template.Number,
                template.Duration);
            return track;
        }

        /// <summary>
        /// Delete an existing track
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTrackAsync(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Deleting track with ID {id}");

            // Make sure the track exists
            var track = await _factory.Tracks.GetAsync(x => x.Id == id);

            // If the track doesn't exist, return a 404
            if (track == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Track with ID {id} not found");
                return NotFound();
            }

            // Delete the track
            await _factory.Tracks.DeleteAsync(id);

            return Ok();
        }
    }
}
