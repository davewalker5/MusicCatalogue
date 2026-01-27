using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class RetailersController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public RetailersController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all the retailers in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Retailer>>> GetRetailersAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving list of retailers");

            // Get a list of all retailers in the catalogue
            List<Retailer> retailers = await _factory.Retailers.ListAsync(x => true);

            // If there are no retailers, return a no content response
            if (!retailers.Any())
            {
                return NoContent();
            }

            return retailers;
        }

        /// <summary>
        /// Return retailer details given a retailer ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Retailer>> GetRetailerByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving retailer with ID {id}");

            var retailer = await _factory.Retailers.GetAsync(x => x.Id == id);

            if (retailer == null)
            {
                _logger.LogMessage(Severity.Error, $"Retailer with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved retailer {retailer}");
            return retailer;
        }

        /// <summary>
        /// Add a retailer to the catalogue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Retailer>> AddRetailerAsync([FromBody] Retailer template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding retailer {template}");
            var retailer = await _factory.Retailers.AddAsync(
                template.Name,
                template.Address1,
                template.Address2,
                template.Town,
                template.County,
                template.PostCode,
                template.Country,
                template.Latitude,
                template.Longitude,
                template.WebSite,
                template.ArtistDirect);
            return retailer;
        }

        /// <summary>
        /// Update an existing retailer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Retailer?>> UpdateRetailerAsync([FromBody] Retailer template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating retailer {template}");
            var retailer = await _factory.Retailers.UpdateAsync(
                template.Id,
                template.Name,
                template.Address1,
                template.Address2,
                template.Town,
                template.County,
                template.PostCode,
                template.Country,
                template.Latitude,
                template.Longitude,
                template.WebSite,
                template.ArtistDirect);
            return retailer;
        }

        /// <summary>
        /// Delete an existing retailer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRetailerAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting retailer with ID {id}");

            // Make sure the retailer exists
            var retailer = await _factory.Retailers.GetAsync(x => x.Id == id);

            // If the retailer doesn't exist, return a 404
            if (retailer == null)
            {
                _logger.LogMessage(Severity.Error, $"Retailer with ID {id} not found");
                return NotFound();
            }

            try
            {
                // Delete the retailer
                await _factory.Retailers.DeleteAsync(id);
            }
            catch (RetailerInUseException)
            {
                // Retailer is in use so this is a bad request
                _logger.LogMessage(Severity.Error, $"Retailer with ID {id} has purchases associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}