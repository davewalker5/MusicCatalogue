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
        [Route("artist/{artistId}")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumsByArtistAsync(int artistId)
        {
            List<Album> albums = await _factory.Albums.ListAsync(x => x.ArtistId == artistId);

            if (!albums.Any())
            {
                return NoContent();
            }

            return albums;
        }

        /// <summary>
        /// 
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
