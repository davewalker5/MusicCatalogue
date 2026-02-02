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
        private readonly IMusicLogger _logger;

        public SearchController(IMusicCatalogueFactory factory, IAlbumLookupManager manager, IMusicLogger logger)
        {
            _factory = factory;
            _manager = manager;
            _logger = logger;
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
            _logger.LogMessage(Severity.Debug, $"Searching for '{albumTitle}' by {artistName} - results will be stored in the {catalogue}");

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
            _logger.LogMessage(Severity.Debug, $"Searching closest artists using criteria {criteria}");
            var closest = await _factory.ArtistSimilarityCalculator.GetClosestArtistsAsync(criteria, criteria.ArtistId, criteria.TopN, true);
            return closest;
        }

        /// <summary>
        /// Pick albums matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pick")]
        public async Task<ActionResult<List<PickedAlbum>>> GetRandomAlbumsAsync([FromBody] AlbumSelectionCriteria criteria)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving random albums matching criteria {criteria}");
            var pickedAlbums = await _factory.AlbumPicker.PickAsync(criteria);
            return pickedAlbums;
        }

        /// <summary>
        /// Generate a playlist using the specified playlist builder criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("playlist")]
        public async Task<ActionResult<List<Album>>> GeneratePlaylistAsync([FromBody] PlaylistBuilderCriteria criteria)
        {
            _logger.LogMessage(Severity.Debug, $"Generating a playlist using criteria {criteria}");
            var playlist = await _factory.ArtistPlaylistBuilder.BuildPlaylist(criteria.Type, criteria.TimeOfDay, criteria.NumberOfEntries);
            var albums = await _factory.ArtistPlaylistBuilder.PickPlaylistAlbums(playlist);
            return albums;
        }
    }
}
