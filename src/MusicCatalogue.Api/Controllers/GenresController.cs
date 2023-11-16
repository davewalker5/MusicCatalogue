using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;

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
        {
            _factory = factory;
        }

        /// <summary>
        /// Return a list of all the genres in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Genre>>> GetGenresAsync()
        {
            // Get a list of all artists in the catalogue
            List<Genre> genres = await _factory.Genres.ListAsync(x => true);

            // If there are no genres, return a no content response
            if (!genres.Any())
            {
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
            var genre = await _factory.Genres.GetAsync(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

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
            var genre = await _factory.Genres.AddAsync(template.Name);
            return genre;
        }

        /// <summary>
        /// Update an existing genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Genre?>> UpdateRetailerAsync([FromBody] Genre template)
        {
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
            // Make sure the genre exists
            var genre = await _factory.Genres.GetAsync(x => x.Id == id);

            // If the genre doesn't exist, return a 404
            if (genre == null)
            {
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
                return BadRequest();
            }

            return Ok();
        }
    }
}