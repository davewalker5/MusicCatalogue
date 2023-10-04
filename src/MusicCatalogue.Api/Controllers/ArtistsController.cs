using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Logic.Factory;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class ArtistsController : Controller
    {
        private readonly MusicCatalogueFactory _factory;

        public ArtistsController(MusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Artist>>> GetArtistsAsync()
        {
            List<Artist> artists = await _factory.Artists.ListAsync(x => true);

            if (!artists.Any())
            {
                return NoContent();
            }

            return artists;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Artist>> GetArtistByIdAsync(int id)
        {
            var artist = await _factory.Artists.GetAsync(x => x.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }
    }
}
