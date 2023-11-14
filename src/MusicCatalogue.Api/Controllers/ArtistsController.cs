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

        /// <summary>
        /// Return a list of all the artists in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{filter}/{wishlist:bool}")]
        public async Task<ActionResult<List<Artist>>> GetArtistsAsync(string filter, bool wishlist)
        {
            // Get a list of artists mathing the filtering criteria
            List<Artist> artists;
            if (filter == "*")
            {
                artists = await _factory.Artists.ListAsync(x => true);
            }
            else
            {
                artists = await _factory.Artists.ListAsync(x => x.Name.StartsWith(filter));
            }    

            // The artist list includes the albums by that artist, so where an artist has any albums
            // filter them according to the wish list filter
            foreach (var artist in artists)
            {
                if ((artist.Albums != null) && artist.Albums.Any())
                {
                    var filtered = artist.Albums.Where(x => (x.IsWishListItem ?? false) == wishlist).ToList();
                    artist.Albums = filtered;
                }
            }

            // Remove any artists with no albums
            artists.RemoveAll(x => (x.Albums == null) || ((x.Albums != null) && !x.Albums.Any()));

            // If there are no artists, return a no content response
            if (!artists.Any())
            {
                return NoContent();
            }

            return artists;
        }

        /// <summary>
        /// Return artist details given an artist ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
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
