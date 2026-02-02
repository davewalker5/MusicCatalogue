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
    public class GenresController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public GenresController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Return a list of genres matching the specified criteria
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<List<Genre>>> GetGenresAsync(GenreSearchCriteria criteria)
        {
            // Ideally, this method would use the GET verb but as more filtering criteria are added that leads
            // to an increasing number of query string parameters and a very messy URL. So the filter criteria
            // are POSTed in the request body, instead, and bound into a strongly typed criteria object

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving genres matching criteria {criteria}");

            // Retrieve a list of matching genres
            var genres = await _factory.Search.GenreSearchAsync(criteria);

            // If there are no genres, return a no content response
            if (genres == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"No matching genres found");
                return NoContent();
            }

            return genres;
        }

        /// <summary>
        /// Return genre details given a genre ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Genre>> GetGenreByIdAsync(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving genre with ID {id}");

            var genre = await _factory.Genres.GetAsync(x => x.Id == id);

            if (genre == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Genre with ID {id} not found");
                return NotFound();
            }

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieved genre {genre}");
            return genre;
        }

        /// <summary>
        /// Add a genre to the catalogue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Genre>> AddGenreAsync([FromBody] Genre template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Adding genre {template}");
            var genre = await _factory.Genres.AddAsync(template.Name, true);
            return genre;
        }

        /// <summary>
        /// Update an existing genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Genre?>> UpdateGenreAsync([FromBody] Genre template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Updating genre {template}");
            var genre = await _factory.Genres.UpdateAsync(template.Id, template.Name);
            return genre;
        }

        /// <summary>
        /// Delete an existing genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Deleting genre with ID {id}");

            // Make sure the genre exists
            var genre = await _factory.Genres.GetAsync(x => x.Id == id);

            // If the genre doesn't exist, return a 404
            if (genre == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Genre with ID {id} not found");
                return NotFound();
            }

            try
            {
                // Delete the genre
                await _factory.Genres.DeleteAsync(id);
            }
            catch (GenreInUseException)
            {
                // Genre is in use so this is a bad request
                _factory.Logger.LogMessage(Severity.Error, $"Genre with ID {id} has albums associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}