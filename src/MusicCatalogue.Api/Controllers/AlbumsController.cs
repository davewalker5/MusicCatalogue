using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
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

        public AlbumsController(IMusicCatalogueFactory factory)
        {
            _factory = factory;
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
            var album = await _factory.Albums.GetAsync(x => x.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        [HttpGet]
        [Route("random")]
        public async Task<ActionResult<Album?>> GetRandomAlbum()
        {
            var album = await _factory.Albums.GetRandomAsync(x => !(x.IsWishListItem ?? false));
            return album;
        }

        [HttpGet]
        [Route("random/{genreId}")]
        public async Task<ActionResult<Album?>> GetRandomAlbum(int genreId)
        {
            var album = await _factory.Albums.GetRandomAsync(x => !(x.IsWishListItem ?? false) && (x.GenreId == genreId));
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

            // Retrieve a list of matching albums
            var albums = await _factory.Search.AlbumSearchAsync(criteria);

            if (albums == null)
            {
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
            // Check the album exists, first
            var album = await _factory.Albums.GetAsync(x => x.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            // It does, so delete it
            await _factory.Albums.DeleteAsync(id);
            return Ok();
        }
    }
}
