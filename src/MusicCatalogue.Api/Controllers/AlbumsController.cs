using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class AlbumsController : Controller
    {
        private const string OtherGenre = "Other";

        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public AlbumsController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return album details given an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Album>> GetAlbumByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving album with ID {id}");

            var album = await _factory.Albums.GetAsync(x => x.Id == id);

            if (album == null)
            {
                _logger.LogMessage(Severity.Error, $"Album with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved album {album}");
            return album;
        }

        [HttpGet]
        [Route("random")]
        public async Task<ActionResult<Album?>> GetRandomAlbum()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving random album across all genres");
            var album = await _factory.Albums.GetRandomAsync(x => !(x.IsWishListItem ?? false));
            _logger.LogMessage(Severity.Debug, $"Retrieved random album {album}");
            return album;
        }

        [HttpGet]
        [Route("random/{genreId}")]
        public async Task<ActionResult<Album?>> GetRandomAlbum(int genreId)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving random album for the genre with ID {genreId}");
            var album = await _factory.Albums.GetRandomAsync(x => !(x.IsWishListItem ?? false) && (x.GenreId == genreId));
            _logger.LogMessage(Severity.Debug, $"Retrieved random album {album}");
            return album;
        }

        /// <summary>
        /// Return a list of albums for the specified artist, filtering for items that are on/not on
        /// the wishlist based on the arguments
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumsAsync([FromBody] AlbumSearchCriteria criteria)
        {
            // Ideally, this method would use the GET verb but as more filtering criteria are added that leads
            // to an increasing number of query string parameters and a very messy URL. So the filter criteria
            // are POSTed in the request body, instead, and bound into a strongly typed criteria object

            _logger.LogMessage(Severity.Debug, $"Retrieving albums matching criteria {criteria}");

            // Retrieve a list of matching albums
            var albums = await _factory.Search.AlbumSearchAsync(criteria);

            if (albums == null)
            {
                _logger.LogMessage(Severity.Error, $"No matching albums found");
                return NoContent();
            }

            return albums;
        }

        /// <summary>
        /// Add an album from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Album>> AddAlbumAsync([FromBody] Album template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding album {template}");

            // Make sure the "other" Genre exists as a fallback for album updates where no genre is given
            var otherGenre = _factory.Genres.AddAsync(OtherGenre, false);

            // Add the album
            var album = await _factory.Albums.AddAsync(
                template.ArtistId,
                template.GenreId ?? otherGenre.Id,
                template.Title,
                template.Released,
                template.CoverUrl,
                template.IsWishListItem,
                template.Purchased,
                template.Price,
                template.RetailerId);

            // Return the new album
            return album;
        }

        /// <summary>
        /// Update an album from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Album>> UpdateAlbumAsync([FromBody] Album template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating album {template}");

            // Make sure the "other" Genre exists as a fallback for album updates where no genre is given
            var otherGenre = _factory.Genres.AddAsync(OtherGenre, false);

            // Attempt the update
            var album = await _factory.Albums.UpdateAsync(
                template.Id,
                template.ArtistId,
                template.GenreId ?? otherGenre.Id,
                template.Title,
                template.Released,
                template.CoverUrl,
                template.IsWishListItem,
                template.Purchased,
                template.Price,
                template.RetailerId);

            // If the result is NULL, the album doesn't exist
            if (album == null)
            {
                _logger.LogMessage(Severity.Error, $"Album not found");
                return NotFound();
            }

            // Return the updated album
            return album;
        }

        /// <summary>
        /// Delete an album and its tracks given an album ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting album with ID {id}");

            // Check the album exists, first
            var album = await _factory.Albums.GetAsync(x => x.Id == id);
            if (album == null)
            {
                _logger.LogMessage(Severity.Error, $"Album with ID {id} not found");
                return NotFound();
            }

            // It does, so delete it
            _logger.LogMessage(Severity.Debug, $"Deleting album {album}");
            await _factory.Albums.DeleteAsync(id);
            return Ok();
        }
    }
}
