using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class ArtistMoodsController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public ArtistMoodsController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Add an artist/mood mapping from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ArtistMood>> AddArtistMoodAsync([FromBody] ArtistMood template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding artist/mood mapping {template}");
            var mood = await _factory.ArtistMoods.AddAsync(template.ArtistId, template.MoodId);
            return mood;
        }

        /// <summary>
        /// Delete an artist/mood mapping given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteArtistMood(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting artist/mood mapping with ID {id}");

            // Check the mood exists, first
            var context = _factory.Context as MusicCatalogueDbContext;
            var mood = context!.ArtistMoods.FirstOrDefault(x => x.Id == id);
            if (mood == null)
            {
                _logger.LogMessage(Severity.Error, $"Artist/mood mapping with ID {id} not found");
                return NotFound();
            }

            await _factory.ArtistMoods.DeleteAsync(id);
            return Ok();
        }
    }
}
