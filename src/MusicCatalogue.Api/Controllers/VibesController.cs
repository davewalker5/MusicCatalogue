using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class VibesController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public VibesController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all the vibes in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Vibe>>> GetVibesAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving list of vibes");

            var vibes = await _factory.Vibes.ListAsync(x => true);

            if (vibes == null)
            {
                return NoContent();
            }

            return vibes;
        }

        /// <summary>
        /// Return vibe details given a vibe ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Vibe>> GetVibeByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving vibe with ID {id}");

            var vibe = await _factory.Vibes.GetAsync(x => x.Id == id);

            if (vibe == null)
            {
                _logger.LogMessage(Severity.Error, $"Vibe with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved vibe {vibe}");
            return vibe;
        }

        /// <summary>
        /// Add a vibe from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Vibe>> AddVibeAsync([FromBody] Vibe template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding vibe {template}");
            var vibe = await _factory.Vibes.AddAsync(template.Name);
            return vibe;
        }

        /// <summary>
        /// Update a vibe from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Vibe?>> UpdateVibeAsync([FromBody] Vibe template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating vibe {template}");
            var vibe = await _factory.Vibes.UpdateAsync(template.Id, template.Name);
            return vibe;
        }

        /// <summary>
        /// Delete a vibe given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteVibe(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting vibe with ID {id}");

            // Check the vibe exists, first
            var vibe = await _factory.Vibes.GetAsync(x => x.Id == id);
            if (vibe == null)
            {
                _logger.LogMessage(Severity.Error, $"Vibe with ID {id} not found");
                return NotFound();
            }

            try
            {
                // They do, so delete them
                await _factory.Vibes.DeleteAsync(id);
            }
            catch (VibeInUseException)
            {
                // Vibe is in use (have equipment associated with them) so this is a bad request
                _logger.LogMessage(Severity.Error, $"Vibe with ID {id} has artists associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
