using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Playlists;
using System.Web;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IAlbumLookupManager _manager;

        public SearchController(
            IMusicCatalogueFactory factory,
            IAlbumLookupManager manager)
        {
            _factory = factory;
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
        public async Task<ActionResult<Album>> SearchAsync(string artistName, string albumTitle, bool storeInWishList)
        {
            // Decode the search criteria
            var decodedArtistName = HttpUtility.UrlDecode(artistName);
            var decodedAlbumTitle = HttpUtility.UrlDecode(albumTitle);

            var catalogue = storeInWishList ? "wish list" : "main catalogue";
            _factory.Logger.LogMessage(Severity.Debug, $"Searching for '{albumTitle}' by {artistName} - results will be stored in the {catalogue}");

            // Perform the lookup
            var album = await _manager.LookupAlbum(decodedArtistName, decodedAlbumTitle, storeInWishList);
            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        /// <summary>
        /// Return the "top N" artists closest to a target artist based on the style characteristics
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="topN"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("closest")]
        public async Task<ActionResult<List<ClosestArtist>>> ClosestArtistsAsync([FromBody] ClosestArtistSearchCriteria criteria)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Searching closest artists using criteria {criteria}");
            var closest = await _factory.ArtistSimilarityCalculator.GetClosestArtistsAsync(criteria, criteria.ArtistId, criteria.TopN, true);
            return closest;
        }
    }
}
