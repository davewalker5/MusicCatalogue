using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;

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
        {
            _factory = factory;
        }

        /// <summary>
        /// Add a track to the catalogue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Track>> AddTrackAsync([FromBody] Track template)
        {
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
            // Make sure the track exists
            var track = await _factory.Tracks.GetAsync(x => x.Id == id);

            // If the track doesn't exist, return a 404
            if (track == null)
            {
                return NotFound();
            }

            // Delete the track
            await _factory.Tracks.DeleteAsync(id);

            return Ok();
        }
    }
}
