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
    public class ArtistsController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public ArtistsController(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Artist>>> GetArtistsAsync()
        {
            // Get a list of all artists in the catalogue
            List<Artist> artists = await _factory.Artists.ListAsync(x => true);

            // If there are no artists, return a no content response
            if (!artists.Any())
            {
                return NoContent();
            }

            // Populate the artist statistics
            await _factory.Statistics.PopulateArtistStatistics(artists);

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
