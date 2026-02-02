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

        public ManufacturersController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Return a list of all the manufacturers in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Manufacturer>>> GetManufacturersAsync()
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving list of manufacturers");

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
            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving manufacturer with ID {id}");

            var manufacturer = await _factory.Manufacturers.GetAsync(x => x.Id == id);

            if (manufacturer == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} not found");
                return NotFound();
            }

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieved manufacturer {manufacturer}");
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
            _factory.Logger.LogMessage(Severity.Debug, $"Adding manufacturer {template}");
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
            _factory.Logger.LogMessage(Severity.Debug, $"Updating manufacturer {template}");
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
            _factory.Logger.LogMessage(Severity.Debug, $"Deleting manufacturer with ID {id}");

            // Check the manufacturer exists, first
            var manufacturer = await _factory.Manufacturers.GetAsync(x => x.Id == id);
            if (manufacturer == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} not found");
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
                _factory.Logger.LogMessage(Severity.Error, $"Manufacturer with ID {id} has equipment associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
