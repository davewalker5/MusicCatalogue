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

        /// <summary>
        /// Look up an album given the album title and artist name. Store the album and its tracks either
        /// in the wish list or the main catalogue
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumTitle"></param>
        /// <param name="storeInWishList"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{artistName}/{albumTitle}/{storeInWishList}")]
        public async Task<ActionResult<Album>> Search(string artistName, string albumTitle, bool storeInWishList)
        {
            // Decode the search criteria
            var decodedArtistName = HttpUtility.UrlDecode(artistName);
            var decodedAlbumTitle = HttpUtility.UrlDecode(albumTitle);

            // Perform the lookup
            var album = await _manager.LookupAlbum(decodedArtistName, decodedAlbumTitle, storeInWishList);
            if (album == null)
            {
                return NotFound();
            }

            return album;
        }
    }
}
