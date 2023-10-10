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
    }
}
