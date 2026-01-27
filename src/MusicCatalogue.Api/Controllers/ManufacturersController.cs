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
    public class ManufacturersController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public ManufacturersController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all the manufacturers in the catalogue matching the filter criteria in the request body
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Manufacturer>>> GetManufacturersAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving list of manufacturers");

            var manufacturers = await _factory.Manufacturers.ListAsync(x => true);

            if (manufacturers == null)
            {
                return NoContent();
            }

            return manufacturers;
        }

        /// <summary>
        /// Return manufacturer details given a manufacturer ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Manufacturer>> GetManufacturerByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving manufacturer with ID {id}");

            var manufacturer = await _factory.Manufacturers.GetAsync(x => x.Id == id);

            if (manufacturer == null)
            {
                _logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved manufacturer {manufacturer}");
            return manufacturer;
        }

        /// <summary>
        /// Add a manufacturer from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Manufacturer>> AddManufacturerAsync([FromBody] Manufacturer template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding manufacturer {template}");
            var manufacturer = await _factory.Manufacturers.AddAsync(template.Name);
            return manufacturer;
        }

        /// <summary>
        /// Update a manufacturer from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Manufacturer?>> UpdateManufacturerAsync([FromBody] Manufacturer template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating manufacturer {template}");
            var manufacturer = await _factory.Manufacturers.UpdateAsync(template.Id, template.Name);
            return manufacturer;
        }

        /// <summary>
        /// Delete a manufacturer given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteManufacturer(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting manufacturer with ID {id}");

            // Check the manufacturer exists, first
            var manufacturer = await _factory.Manufacturers.GetAsync(x => x.Id == id);
            if (manufacturer == null)
            {
                _logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} not found");
                return NotFound();
            }

            try
            {
                // They do, so delete them
                await _factory.Manufacturers.DeleteAsync(id);
            }
            catch (ManufacturerInUseException)
            {
                // Manufacturer is in use (have equipment associated with them) so this is a bad request
                _logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} has equipment associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
