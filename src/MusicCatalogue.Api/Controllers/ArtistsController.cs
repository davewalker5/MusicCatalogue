using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class ArtistsController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public ArtistsController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Return a list of all the artists in the catalogue matching the filter criteria in the request body
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<List<Artist>>> GetArtistsAsync([FromBody] ArtistSearchCriteria criteria)
        {
            // Ideally, this method would use the GET verb but as more filtering criteria are added that leads
            // to an increasing number of query string parameters and a very messy URL. So the filter criteria
            // are POSTed in the request body, instead, and bound into a strongly typed criteria object

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving artists matching criteria {criteria}");

            var artists = await _factory.Search.ArtistSearchAsync(criteria);

            if (artists == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"No matching artists found");
                return NoContent();
            }

            return artists;
        }

        /// <summary>
        /// Return artist details given an artist ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Artist>> GetArtistByIdAsync(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving artist with ID {id}");

            var artist = await _factory.Artists.GetAsync(x => x.Id == id, true);

            if (artist == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Artist with ID {id} not found");
                return NotFound();
            }

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieved artist {artist}");
            return artist;
        }

        /// <summary>
        /// Add an artist from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Artist>> AddArtistAsync([FromBody] Artist template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Adding artist {template}");

            // Add the artist
            var artist = await _factory.Artists.AddAsync(
                template.Name,
                template.Energy,
                template.Intimacy,
                template.Warmth,
                template.Vocals,
                template.Ensemble);

            // Return the new artist
            return artist;
        }

        /// <summary>
        /// Update an artist from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Artist?>> UpdateArtistAsync([FromBody] Artist template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Updating artist {template}");

            // Add the artist
            var artist = await _factory.Artists.UpdateAsync(
                template.Id,
                template.Name,
                template.Energy,
                template.Intimacy,
                template.Warmth,
                template.Vocals,
                template.Ensemble);

            // Return the new artist
            return artist;
        }

        /// <summary>
        /// Delete an artist given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Deleting artist with ID {id}");

            // Check the artist exists, first
            var artist = await _factory.Artists.GetAsync(x => x.Id == id, false);
            if (artist == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Artist with ID {id} not found");
                return NotFound();
            }

            try
            {
                // They do, so delete them
                await _factory.Artists.DeleteAsync(id);
            }
            catch (ArtistInUseException)
            {
                // Artist is in use (has albums) so this is a bad request
                _factory.Logger.LogMessage(Severity.Error, $"Artist with ID {id} has albums associated with them and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
