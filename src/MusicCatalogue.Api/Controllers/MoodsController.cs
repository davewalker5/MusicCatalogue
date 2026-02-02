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
    public class MoodsController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public MoodsController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all the moods in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Mood>>> GetMoodsAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving list of moods");

            var moods = await _factory.Moods.ListAsync(x => true);

            if (moods == null)
            {
                return NoContent();
            }

            return moods;
        }

        /// <summary>
        /// Return mood details given a mood ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Mood>> GetMoodByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving mood with ID {id}");

            var mood = await _factory.Moods.GetAsync(x => x.Id == id);

            if (mood == null)
            {
                _logger.LogMessage(Severity.Error, $"Mood with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved mood {mood}");
            return mood;
        }

        /// <summary>
        /// Add a mood from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Mood>> AddMoodAsync([FromBody] Mood template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding mood {template}");
            var mood = await _factory.Moods.AddAsync(
                template.Name,
                template.MorningWeight,
                template.AfternoonWeight,
                template.EveningWeight,
                template.LateWeight);
            return mood;
        }

        /// <summary>
        /// Update a mood from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Mood?>> UpdateMoodAsync([FromBody] Mood template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating mood {template}");
            var mood = await _factory.Moods.UpdateAsync(
                template.Id,
                template.Name,
                template.MorningWeight,
                template.AfternoonWeight,
                template.EveningWeight,
                template.LateWeight);
            return mood;
        }

        /// <summary>
        /// Delete a mood given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMood(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting mood with ID {id}");

            // Check the mood exists, first
            var mood = await _factory.Moods.GetAsync(x => x.Id == id);
            if (mood == null)
            {
                _logger.LogMessage(Severity.Error, $"Mood with ID {id} not found");
                return NotFound();
            }

            try
            {
                // They do, so delete them
                await _factory.Moods.DeleteAsync(id);
            }
            catch (MoodInUseException)
            {
                // Mood is in use (have equipment associated with them) so this is a bad request
                _logger.LogMessage(Severity.Error, $"Mood with ID {id} has artists associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
