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
    public class AlbumsController : Controller
    {
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

        /// <summary>
        /// Return a list of albums for the specified artist, filtering for items that are on/not on
        /// the wishlist based on the arguments
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("artist/{artistId}/{wishlist}")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumsByArtistAsync(int artistId, bool wishlist)
        {
            List<Album> albums;

            // Get the albums matching the specified criteria - the wish list flag is either null, false or true
            if (wishlist)
            {
                albums = await _factory.Albums.ListAsync(x => (x.ArtistId == artistId) && (x.IsWishListItem == true));
            }
            else
            {
                albums = await _factory.Albums.ListAsync(x => (x.ArtistId == artistId) && (x.IsWishListItem != true));
            }

            if (!albums.Any())
            {
                return NoContent();
            }

            return albums;
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
            // Attempt the update
            var album = await _factory.Albums.UpdateAsync(
                template.Id,
                template.ArtistId,
                template.Title,
                template.Released,
                template.Genre,
                template.CoverUrl,
                template.IsWishListItem);

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
