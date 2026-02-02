using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Extensions;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class EnumerationsController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public EnumerationsController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of options for "vocal presence"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("vocalpresence")]
        public ActionResult<List<EnumItem>> GetVocalPresenceOptionsAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving vocal presence options");
            var options = Enum.GetValues<VocalPresence>()
                                .Select(v => new EnumItem((int)v, v.ToName()))
                                .ToList();
            return options;
        }

        /// <summary>
        /// Return a list of options for "ensemble type"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ensembletype")]
        public ActionResult<List<EnumItem>> GetEnsembleTypeOptionsAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving ensemble type options");
            var options = Enum.GetValues<EnsembleType>()
                                .Select(v => new EnumItem((int)v, v.ToName()))
                                .ToList();
            return options;
        }

        /// <summary>
        /// Return a list of options for "playlist type"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("playlisttype")]
        public ActionResult<List<EnumItem>> GetPlaylistTypeOptionsAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving playlist type options");
            var options = Enum.GetValues<PlaylistType>()
                                .Select(v => new EnumItem((int)v, v.ToName()))
                                .ToList();
            return options;
        }

        /// <summary>
        /// Return a list of options for "time of day"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("timeofday")]
        public ActionResult<List<EnumItem>> GetTimeOfDayOptionsAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving time of day options");
            var options = Enum.GetValues<TimeOfDay>()
                                .Select(v => new EnumItem((int)v, v.ToName()))
                                .ToList();
            return options;
        }
    }
}