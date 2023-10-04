using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Web;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IAlbumLookupManager _manager;

        public SearchController(IAlbumLookupManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("{artistName}/{albumTitle}")]
        public async Task<ActionResult<Album>> Search(string artistName, string albumTitle)
        {
            // Decode the search criteria
            var decodedArtistName = HttpUtility.UrlDecode(artistName);
            var decodedAlbumTitle = HttpUtility.UrlDecode(albumTitle);

            // Perform the lookup
            var album = await _manager.LookupAlbum(decodedArtistName, decodedAlbumTitle);
            if (album == null)
            {
                return NotFound();
            }

            return album;
        }
    }
}
